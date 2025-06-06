using Domain.States;
using ErrorOr;
using MediatR;

namespace Application.StateManagement.Generic;

public class SynchronizedRequest : IRequest<ErrorOr<IAppState>>
{
}