namespace Domain.States;

public class AppState: IAppState
{
    public List<int> Numbers { get; set; } = [1, 2, 3];
    public Action<IAppState> AppStateChanged { get; set; }
}