using System.Reflection;
using Application.StateManagement.Common;
using Application.StateManagement.Specific;
using Domain.States;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tests.Integration;
using Xunit.Sdk;

namespace Tests.UnitTests;

[Collection("NonParallel Tests")]
public class AppState1Tests : IClassFixture<MyCustomWebFactory>
{
    private readonly HttpClient _client;
    private readonly IMediator _mediator;
    private readonly MyCustomWebFactory _factory;

    public AppState1Tests(MyCustomWebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _mediator = factory.Services.GetRequiredService<IMediator>();
    }

    [Fact]
    public async Task AppStateHash_DoesNotChange_WhenRead()
    {
        var request1 = new GetAppStateRequest<List<string>>();
        var request2 = new GetAppStateRequest<List<string>>();
        
        
        var response1 = await _mediator.Send(request1);
        var response2 = await _mediator.Send(request2);

        Assert.NotNull(response1);
        Assert.NotNull(response2);
        Assert.Equal(response1.GetHashCode(), response2.GetHashCode());
    }

    [Fact]
    public async Task Modification_Triggers_Callback()
    {
        var singleton = _factory.Services.GetRequiredService<IAppStateWrapper<List<string>>>();
        var triggered = false;

        void ChangeTrigger(IAppState<List<string>> state)
        {
            triggered = true;
        }

        singleton.StateChanged += ChangeTrigger;

        var response1 = await _mediator.Send(new SetAppStateRequest<List<string>>()
            { NewState = new AppState<List<string>>(), LastStateHash = singleton.CurrentState.GetHashCode() });

        Assert.True(triggered);
    }

    [Fact]
    public async Task Modification_Updates_WrapperState()
    {
        
        var singleton = _factory.Services.GetRequiredService<IAppStateWrapper<List<string>>>();
        var initialHash = singleton.CurrentState.GetHashCode();
        
        var response1 = await _mediator.Send(new SetAppStateRequest<List<string>>()
            { NewState = new AppState<List<string>>(), LastStateHash = singleton.CurrentState.GetHashCode() });
        
        Assert.NotEqual(initialHash, singleton.CurrentState.GetHashCode());
        Assert.Equal(response1.GetHashCode(), singleton.CurrentState.GetHashCode());
    }


    [Theory]
    [Repeat(1000)]
    public async Task ThreadCollisionWrites_ThrowsException()
    {
        var singleton = _factory.Services.GetRequiredService<IAppStateWrapper<List<string>>>();
        var initialHash = singleton.CurrentState.GetHashCode();

        var exceptions = new List<Exception>();

        var t1 = Task.Run(async () =>
        {
            //await Task.Delay(TimeSpan.FromMilliseconds(Random.Shared.NextDouble() * 20));

            await _mediator.Send(new SetAppStateRequest<List<string>>()
                { NewState = new AppState<List<string>>(), LastStateHash = initialHash });
        });

        var t2 = Task.Run(async () => await _mediator.Send(new SetAppStateRequest<List<string>>()
            { NewState = new AppState<List<string>>(), LastStateHash = initialHash }));

        try
        {
            await t1;
            await t2;
        }
        catch (Exception e)
        {
            exceptions.Add(e);
        }

        Assert.NotEmpty(exceptions);
        Assert.Equal(1, exceptions.Count);
        Assert.IsType<HashOutdatedException>(exceptions[0]);

    }

    [Theory]
    [Repeat(1000)]
    public async Task ReAttemptingConflictedWrites_WorksEventually()
    {
        var singleton = _factory.Services.GetRequiredService<IAppStateWrapper<List<string>>>();
        var initialHash = singleton.CurrentState.GetHashCode();

        var exceptions = new List<Exception>();

        var t1 = Task.Run(async () => await _mediator.Send(new SetAppStateRequest<List<string>>()
            { NewState = new AppState<List<string>>(), LastStateHash = initialHash }));


        var t2 = Task.Run(async () => await _mediator.Send(new SetAppStateRequest<List<string>>()
            { NewState = new AppState<List<string>>(), LastStateHash = initialHash }));

        try
        {
            await t1;
        }
        catch (HashOutdatedException e)
        {
            try
            {
                var t3 = Task.Run(async () => await _mediator.Send(new SetAppStateRequest<List<string>>()
                    { NewState = new AppState<List<string>>(), LastStateHash = singleton.CurrentState.GetHashCode() }));
                await t3;
            }
            catch (Exception eInner)
            {
                exceptions.Add(eInner);
            }
        }
        catch (Exception e)
        {
            exceptions.Add(e);
        }

        try
        {
            await t2;
        }
        catch (HashOutdatedException e)
        {
            try
            {
                var t3 = Task.Run(async () => await _mediator.Send(new SetAppStateRequest<List<string>>()
                    { NewState = new AppState<List<string>>(), LastStateHash = singleton.CurrentState.GetHashCode() }));
                await t3;
            }
            catch (Exception eInner)
            {
                exceptions.Add(eInner);
            }
        }
        catch (Exception e)
        {
            exceptions.Add(e);
        }
        
        Assert.Empty(exceptions);
    }
}


public class RepeatAttribute : DataAttribute
{
    private readonly int _count;

    public RepeatAttribute(int count)
    {
        if (count < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(count),
                "Repeat count must be greater than 0.");
        }

        _count = count;
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        return Enumerable.Repeat(new object[0], _count);
    }
}

[CollectionDefinition("NonParallel Tests", DisableParallelization = true)]
public class NonParallelCollectionDefinition
{
}
