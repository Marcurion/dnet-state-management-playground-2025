using Application.StateManagement.AppState1.Generic;
using Domain.States;

namespace Application.StateManagement.AppState1.Specific;

public class SetAppState1Request : AppState1ModificationRequest
{
    public IAppState1 NewState { get; set; }
}