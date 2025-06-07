using Domain.States;

namespace Application.StateManagement.Generic;

/// <summary>
/// Generic base class for state modification requests
/// </summary>
/// <typeparam name="T">The state type</typeparam>
public abstract class AppStateModificationRequest<T> : AppStateRequest<T> where T : class, IAppState<T>
{
    /// <summary>
    /// Hash of the last known state for optimistic concurrency control
    /// </summary>
    public int LastStateHash { get; set; }
}