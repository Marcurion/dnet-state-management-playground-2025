using Application.StateManagement.AppState1.Generic;
using Domain.States;

namespace Application.StateManagement.AppState1.Specific;

public class SetAppState1Request<T> : AppState1ModificationRequest<T>
{
    public IAppState1<T> NewState { get; set; }
}