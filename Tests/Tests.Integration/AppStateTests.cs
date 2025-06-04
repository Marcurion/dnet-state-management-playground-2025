using System.Reflection;
using Application.StateManagement.Specific;
using Domain.States;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tests.Integration;
using Xunit.Sdk;

namespace Tests.UnitTests;

[Collection("NonParallel Tests")]
public class AppStateTests : IClassFixture<MyCustomWebFactory>
{
    private readonly HttpClient _client;
    private readonly IMediator _mediator;
    private readonly MyCustomWebFactory _factory;

    public AppStateTests(MyCustomWebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _mediator = factory.Services.GetRequiredService<IMediator>();
    }

    [Fact]
    public async Task AppStateHash_DoesNotChange_WhenRead()
    {
        var request1 = new GetAppStateRequest();
        var request2 = new GetAppStateRequest();

        var response1 = await _mediator.Send(request1);
        var response2 = await _mediator.Send(request2);

        Assert.False(response1.IsError);
        Assert.False(response2.IsError);
        Assert.NotNull(response1.Value);
        Assert.NotNull(response2.Value);
        Assert.Equal(response1.Value.GetHashCode(), response2.Value.GetHashCode());
    }

    [Fact]
    public async Task Modification_Triggers_Callback()
    {
        var singleton = _factory.Services.GetRequiredService<IAppStateWrapper>();
        var triggered = false;

        void ChangeTrigger(IAppState state)
        {
            triggered = true;
        }

        singleton.CurrentState.AppStateChanged += ChangeTrigger;

        var response1 = await _mediator.Send(new SetAppStateRequest()
            { NewState = new AppState(), LastStateHash = singleton.CurrentState.GetHashCode() });

        Assert.True(triggered);
    }

    [Fact]
    public async Task Modification_Overwrites_StateInstance()
    {
        var singleton = _factory.Services.GetRequiredService<IAppStateWrapper>();
        var initialHash = singleton.CurrentState.GetHashCode();

        var newState = new AppState();
        var newHash = newState.GetHashCode();

        var response1 = await _mediator.Send(new SetAppStateRequest()
            { NewState = newState, LastStateHash = initialHash });

        Assert.Equal(response1.Value.GetHashCode(), newHash);
        Assert.Equal(singleton.CurrentState.GetHashCode(), newHash);
    }

    [Theory]
    [Repeat(1000)]
    public async Task ThreadCollisionWrites_ThrowsException()
    {
        var singleton = _factory.Services.GetRequiredService<IAppStateWrapper>();
        var initialHash = singleton.CurrentState.GetHashCode();

        var exceptions = new List<Exception>();

        var t1 = Task.Run(async () =>
        {
            //await Task.Delay(TimeSpan.FromMilliseconds(Random.Shared.NextDouble() * 20));

            await _mediator.Send(new SetAppStateRequest()
                { NewState = new AppState(), LastStateHash = initialHash });
        });

        var t2 = Task.Run(async () => await _mediator.Send(new SetAppStateRequest()
            { NewState = new AppState(), LastStateHash = initialHash }));

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
        Assert.IsType<ArgumentException>(exceptions[0]);
        Assert.StartsWith("The provided hash differs", exceptions[0].Message);
        //await Assert.ThrowsAsync<ArgumentException>(async () => await t2);
        //Assert.NotNull(t1.Exception);
        //var ex2 = Assert.Throws<AggregateException>(t2.Wait);
    }

    [Theory]
    [Repeat(1000)]
    public async Task ReAttemptingWrites_WorksEventually()
    {
        var singleton = _factory.Services.GetRequiredService<IAppStateWrapper>();
        var initialHash = singleton.CurrentState.GetHashCode();

        var exceptions = new List<Exception>();

        var t1 = Task.Run(async () => await _mediator.Send(new SetAppStateRequest()
            { NewState = new AppState(), LastStateHash = initialHash }));


        var t2 = Task.Run(async () => await _mediator.Send(new SetAppStateRequest()
            { NewState = new AppState(), LastStateHash = initialHash }));

        try
        {
            await t1;
        }
        catch (ArgumentException e)
        {
            try
            {
                var t3 = Task.Run(async () => await _mediator.Send(new SetAppStateRequest()
                    { NewState = new AppState(), LastStateHash = singleton.CurrentState.GetHashCode() }));
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
        catch (ArgumentException e)
        {
            try
            {
                var t3 = Task.Run(async () => await _mediator.Send(new SetAppStateRequest()
                    { NewState = new AppState(), LastStateHash = singleton.CurrentState.GetHashCode() }));
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