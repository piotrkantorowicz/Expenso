using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared;
using Expenso.BudgetSharing.Domain.Shared.Base;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Aggregates;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions;

public sealed class BudgetPermission : IAggregateRoot
{
    private readonly DomainEventsSource _domainEventsSource;
    private readonly IMessageContextFactory _messageContextFactory;

    // ReSharper disable once UnusedMember.Local
    // Required for EF Core
    private BudgetPermission()
    {
        Id = default!;
        BudgetId = default!;
        OwnerId = default!;
        _domainEventsSource = new DomainEventsSource();
        _messageContextFactory = MessageContextFactoryResolver.Resolve();
        Permissions = new List<Permission>();
    }

    private BudgetPermission(BudgetPermissionId id, BudgetId budgetId, PersonId ownerId)
    {
        Id = id;
        BudgetId = budgetId;
        OwnerId = ownerId;
        _domainEventsSource = new DomainEventsSource();
        _messageContextFactory = MessageContextFactoryResolver.Resolve();
        Permissions = new List<Permission>();
    }

    public BudgetPermissionId Id { get; }

    public BudgetId BudgetId { get; }

    public PersonId OwnerId { get; }

    public ICollection<Permission> Permissions { get; }

    public Blocker? Blocker { get; private set; }

    public IReadOnlyCollection<IDomainEvent> GetUncommittedChanges()
    {
        return _domainEventsSource.GetDomainEvents();
    }

    public static BudgetPermission Create(BudgetPermissionId budgetPermissionId, BudgetId budgetId, PersonId ownerId)
    {
        return new BudgetPermission(id: budgetPermissionId, budgetId: budgetId, ownerId: ownerId);
    }

    public static BudgetPermission Create(BudgetId budgetId, PersonId ownerId)
    {
        return new BudgetPermission(id: BudgetPermissionId.New(value: Guid.NewGuid()), budgetId: budgetId,
            ownerId: ownerId);
    }

    public void AddPermission(PersonId participantId, PermissionType permissionType)
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(BusinessRule: new BudgetMustHasDistinctPermissionsForUsers(budgetId: BudgetId,
                participantId: participantId, permissions: Permissions)),
            new BusinesRuleCheck(
                BusinessRule: new BudgetCanHasOnlyOneOwnerPermission(budgetId: BudgetId, permissionType: permissionType,
                    permissions: Permissions), ThrowException: true),
            new BusinesRuleCheck(
                BusinessRule: new BudgetCanHasOnlyOwnerPermissionForItsOwner(budgetId: BudgetId,
                    participantId: participantId, ownerId: OwnerId, permissionType: permissionType),
                ThrowException: true)
        ]);

        Permissions.Add(item: Permission.Create(participantId: participantId, permissionType: permissionType));

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionGrantedEvent(
            MessageContext: _messageContextFactory.Current(), OwnerId: OwnerId, ParticipantId: participantId,
            PermissionType: permissionType));
    }

    public void RemovePermission(PersonId participantId)
    {
        Permission? permission = Permissions.SingleOrDefault(predicate: x => x.ParticipantId == participantId);

        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(
                BusinessRule: new BudgetMustContainsPermissionForProvidedUser(budgetId: BudgetId,
                    participantId: participantId, permission: permission), ThrowException: true),
            new BusinesRuleCheck(
                BusinessRule: new OwnerPermissionCannotBeRemoved(budgetId: BudgetId,
                    permissionType: permission?.PermissionType), ThrowException: true)
        ]);

        Permissions.Remove(item: permission ?? throw new ArgumentException(message: nameof(permission)));

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionWithdrawnEvent(
            MessageContext: _messageContextFactory.Current(), OwnerId: OwnerId, ParticipantId: permission.ParticipantId,
            PermissionType: permission.PermissionType));
    }

    public void Block(IClock? clock = null)
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(
                BusinessRule: new BudgetPermissionCannotBeBlockedIfItIsAlreadyBlocked(budgetPermissionId: Id,
                    blockInfo: Blocker))
        ]);

        Blocker = Blocker.Block(dateTime: clock?.UtcNow ?? DateTimeOffset.UtcNow);

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionBlockedEvent(
            MessageContext: _messageContextFactory.Current(), OwnerId: OwnerId,
            BlockDate: DateAndTime.New(value: Blocker.BlockDate.GetValueOrDefault(defaultValue: DateTimeOffset.UtcNow)),
            Permissions: Permissions.ToList().AsReadOnly()));
    }

    public void Unblock()
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(
                BusinessRule: new BudgetPermissionCannotBeUnblockedIfItIsNotBlocked(budgetPermissionId: Id,
                    blockInfo: Blocker))
        ]);

        Blocker = null;

        _domainEventsSource.AddDomainEvent(domainEvent: new BudgetPermissionUnblockedEvent(
            MessageContext: _messageContextFactory.Current(), OwnerId: OwnerId,
            Permissions: Permissions.ToList().AsReadOnly().ToList().AsReadOnly()));
    }
}