using MediatR;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.Notifications.Commands.MarkAsRead;

public class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public MarkAsReadCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await _unitOfWork.Notifications.GetByIdAsync(request.NotificationId, cancellationToken);
        if (notification == null)
            throw new KeyNotFoundException($"Notification with ID {request.NotificationId} not found.");

        notification.MarkAsRead();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
