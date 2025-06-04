using Domain.States;

namespace Application.StateManagement.Generic;

public class AppStateRequest: SynchronizedRequest
{
    internal IAppState InternalLatestState { get; set; }
}