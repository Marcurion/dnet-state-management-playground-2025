namespace Domain.States;

public class AppState1 : IAppState1<List<string>>
{
    public List<string> Items { get; set; } = ["3", "7", "11"];
}