namespace Domain.States;

/// <summary>
/// Base interface for all application states
/// </summary>
public interface IAppState
{
}

/// <summary>
/// Generic interface for typed application states
/// </summary>
/// <typeparam name="T">The concrete state type</typeparam>
public interface IAppState<T> : IAppState where T : class, IAppState<T>
{
}