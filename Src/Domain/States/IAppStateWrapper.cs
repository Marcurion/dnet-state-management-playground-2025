namespace Domain.States;

public interface IAppStateWrapper
{
    public IAppState CurrentState { get; set; }
}