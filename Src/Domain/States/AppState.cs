namespace Domain.States;

public record AppState<T>( T Value) : IAppState<T>;