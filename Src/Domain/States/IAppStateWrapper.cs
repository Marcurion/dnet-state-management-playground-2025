namespace Domain.States;

/// <summary>
/// Generic interface for application state wrappers
/// </summary>
/// <typeparam name="T">The state type</typeparam>
public interface IAppStateWrapper<T> where T : class, IAppState<T>
{
    /// <summary>
    /// The current state instance
    /// </summary>
    T CurrentState { get; set; }
    
    /// <summary>
    /// Event triggered when state changes
    /// </summary>
    Action<T>? StateChanged { get; set; }
}