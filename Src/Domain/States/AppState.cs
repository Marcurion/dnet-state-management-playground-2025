namespace Domain.States;

public class AppState<T> : IAppState<T>
{
    public T Value { get; set; }
}