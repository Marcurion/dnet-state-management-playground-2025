namespace Domain.States;

public interface IAppState
{
    public List<int> Numbers { get; set; }
    public Action<IAppState> AppStateChanged { get; set; }
}