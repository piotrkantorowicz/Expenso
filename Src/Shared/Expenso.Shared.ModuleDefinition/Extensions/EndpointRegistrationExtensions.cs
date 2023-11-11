namespace Expenso.Shared.ModuleDefinition.Extensions;

public static class EndpointRegistrationExtensions
{
    internal static EndpointRegistration WithLeadingSlash(this EndpointRegistration endpointRegistration)
    {
        if (endpointRegistration.Pattern.Length == 0)
        {
            return endpointRegistration;
        }

        if (endpointRegistration.Pattern[0] != '/')
        {
            endpointRegistration = endpointRegistration with { Pattern = '/' + endpointRegistration.Pattern };
        }

        return endpointRegistration;
    }
}