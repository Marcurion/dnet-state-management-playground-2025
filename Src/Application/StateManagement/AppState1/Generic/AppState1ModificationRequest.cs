namespace Application.StateManagement.AppState1.Generic;

public class AppState1ModificationRequest<T> : AppState1Request<T>
{
    public int LastStateHash;
}