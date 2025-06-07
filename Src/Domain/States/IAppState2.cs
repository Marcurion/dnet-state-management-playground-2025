namespace Domain.States;

public interface IAppState2 : IAppState<AppState2>
{
    public List<string> Items { get; set; }
}