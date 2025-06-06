using Application.StateManagement.AppState2.Generic;
using Domain.States;

namespace Application.StateManagement.AppState2.Specific;

public class SetAppState2Request : AppState2ModificationRequest
{
    public IAppState2 NewState { get; set; }
}