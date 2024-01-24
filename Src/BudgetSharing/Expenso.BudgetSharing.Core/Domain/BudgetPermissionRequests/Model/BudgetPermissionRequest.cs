using Expenso.BudgetSharing.Core.Domain.BudgetPermissionRequests.Model.ValueObjects;
using Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Model.ValueObjects;
using Expenso.BudgetSharing.Core.Domain.Shared;

namespace Expenso.BudgetSharing.Core.Domain.BudgetPermissionRequests.Model;

internal sealed class BudgetPermissionRequest
{
    // ReSharper disable once UnusedMember.Local
    // Required by EF Core   
    private BudgetPermissionRequest() : this(BudgetPermissionRequestId.CreateDefault(), BudgetId.CreateDefault(),
        PersonId.CreateDefault(), PermissionType.Unknown, BudgetPermissionRequestStatus.Unknown,
        DateTimeOffset.MinValue)
    {
    }

    private BudgetPermissionRequest(BudgetPermissionRequestId id, BudgetId budgetId, PersonId participantId,
        PermissionType permissionType, BudgetPermissionRequestStatus status, DateTimeOffset expirationDate)
    {
        Id = id;
        BudgetId = budgetId;
        ParticipantId = participantId;
        PermissionType = permissionType;
        Status = status;
        ExpirationDate = expirationDate;
    }

    public BudgetPermissionRequestId Id { get; }

    public BudgetId BudgetId { get; }

    public PersonId ParticipantId { get; }

    public PermissionType PermissionType { get; }

    public BudgetPermissionRequestStatus Status { get; private set; }

    public DateTimeOffset ExpirationDate { get; private set; }

    public static BudgetPermissionRequest Create(BudgetId budgetId, PersonId personId, PermissionType permissionType,
        DateTimeOffset expirationDate)
    {
        return new BudgetPermissionRequest(Guid.NewGuid(), budgetId, personId, permissionType,
            BudgetPermissionRequestStatus.Pending, expirationDate);
    }

    public void Confirm()
    {
        if (!Status.IsPending())
        {
            throw new InvalidOperationException($"Cannot accept a budget permission request with status {Status}");
        }

        Status = BudgetPermissionRequestStatus.Confirmed;
    }

    public void Cancel()
    {
        if (!Status.IsPending())
        {
            throw new InvalidOperationException($"Cannot cancel a budget permission request with status {Status}");
        }

        Status = BudgetPermissionRequestStatus.Cancelled;
    }

    public void Expire()
    {
        if (!Status.IsPending())
        {
            throw new InvalidOperationException($"Cannot expire a budget permission request with status {Status}");
        }

        Status = BudgetPermissionRequestStatus.Expired;
    }
}