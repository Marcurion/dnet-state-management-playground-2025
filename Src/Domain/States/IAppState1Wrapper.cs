namespace Domain.States;

public interface IAppState1Wrapper
{
    public IAppState1 CurrentState { get; set; }
    
    public Action<IAppState1> StateChanged { get; set; }
}