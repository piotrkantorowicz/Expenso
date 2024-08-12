namespace Expenso.Shared.System.Tasks;

public static class TaskExtensions
{
    public static T RunAsSync<T>(this Task<T> task)
    {
        return task.ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();
    }

    public static void RunAsSync(this Task task)
    {
        task.ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();
    }
}