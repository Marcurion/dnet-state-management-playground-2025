namespace Domain.States;

public class AppState2 : IAppState2
{
    public List<string> Items { get; set; } = ["Green", "Red", "Blue"];
}