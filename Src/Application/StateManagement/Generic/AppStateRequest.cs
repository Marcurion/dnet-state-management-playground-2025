using Domain.States;

namespace Application.StateManagement.Generic;

public class AppStateRequest: SynchronizedRequest
{
    public IAppState InternalLatestState { get; set; }
}