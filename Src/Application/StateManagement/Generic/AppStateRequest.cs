using Domain.States;
using MediatR;

namespace Application.StateManagement.Generic;

/// <summary>
/// Generic base class for all app state requests
/// </summary>
/// <typeparam name="T">The state type</typeparam>
public abstract class AppStateRequest<T> : IRequest<T> where T : class, IAppState<T>
{
    /// <summary>
    /// Internal state injected by pipeline. Should not be set by client code.
    /// </summary>
    internal T? InternalLatestState { get; set; }
}