namespace Domain.States;

public interface IAppState1Wrapper<T>
{
    public IAppState1<T> CurrentState { get; set; }
    
    public Action<IAppState1<T>> StateChanged { get; set; }
}