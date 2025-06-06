namespace Domain.States;

public interface IAppState2Wrapper
{
    public IAppState2 CurrentState { get; set; }
    
    public Action<IAppState2> StateChanged { get; set; }
}