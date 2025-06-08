namespace Domain.States;

public interface IAppState<T>
{
    public T Value { get; init; }
}