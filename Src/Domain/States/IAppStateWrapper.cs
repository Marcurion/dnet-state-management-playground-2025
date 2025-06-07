namespace Domain.States;

public interface IAppStateWrapper<T>
{
    public IAppState<T> CurrentState { get; set; }
    
    public Action<IAppState<T>> StateChanged { get; set; }
}