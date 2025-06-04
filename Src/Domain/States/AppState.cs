namespace Domain.States;

public class AppState: IAppState
{
    public List<int> Numbers { get; set; }
    public Action<IAppState> AppStateChanged { get; set; }
}