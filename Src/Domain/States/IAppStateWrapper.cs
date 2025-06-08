namespace Domain.States;

public interface IAppStateWrapper<T>
{
    public AppState<T> CurrentState { get; set; }
    
    public Action<AppState<T>> StateChanged { get; set; }
}