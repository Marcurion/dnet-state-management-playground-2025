namespace Domain.States;

public class AppState1 : IAppState<List<string>>
{
    public List<string> Items { get; set; } = ["Red", "Blue", "Green"];
}