using Domain.States;
using MediatR;
using ErrorOr;

namespace Application.StateManagement.Generic;

public class SynchronizedRequest : IRequest<ErrorOr<IAppState>>
{
    
}