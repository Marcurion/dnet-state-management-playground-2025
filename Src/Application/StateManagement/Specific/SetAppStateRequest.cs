using Application.StateManagement.Generic;
using Domain.States;

namespace Application.StateManagement.Specific;

public class SetAppStateRequest : AppStateModificationRequest
{
    public IAppState NewState { get; set; }
}