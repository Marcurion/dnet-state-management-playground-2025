namespace Application.StateManagement.Generic;

public class AppStateModificationRequest<T> : AppStateRequest<T>
{
    public int LastStateHash;
}