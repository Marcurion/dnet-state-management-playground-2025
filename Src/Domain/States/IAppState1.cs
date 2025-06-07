namespace Domain.States;

public interface IAppState1 : IAppState<AppState1>
{
    public List<string> Items { get; set; }
}