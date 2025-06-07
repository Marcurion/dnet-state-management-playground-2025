namespace Domain.States;

public interface IAppState1<T>
{
    public T Items { get; set; }
}