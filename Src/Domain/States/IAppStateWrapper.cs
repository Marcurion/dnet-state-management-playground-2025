namespace Domain.States;

public interface IAppStateWrapper<T>
{
    
    // Notable: Maybe not expose the state for consumers of the interface, they should use MediatR,
    // but would require to cache/duplicate parts of the state in the view
    // instead of building directly from this interface 
    public AppState<T> CurrentState { get;  }
    
    public Action<AppState<T>> StateChanged { get; set; }
}