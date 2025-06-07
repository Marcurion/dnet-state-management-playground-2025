namespace Domain.States;

public interface IAppState<T>
{
    public T Items { get; set; }
}