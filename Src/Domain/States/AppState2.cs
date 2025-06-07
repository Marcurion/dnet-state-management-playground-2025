namespace Domain.States;

public class AppState2 : IAppState<List<int>>
{
    public List<int> Items { get; set; } = [1, 2, 3];
}