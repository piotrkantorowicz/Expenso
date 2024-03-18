using Expenso.Shared.Commands;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;

internal sealed class RegisterJobCommandHandler : ICommandHandler<RegisterJobCommand>
{
    private readonly IJobRepository _jobRepository;

    public RegisterJobCommandHandler(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
    }

    public async Task HandleAsync(RegisterJobCommand command, CancellationToken cancellationToken)
    {
    }
}