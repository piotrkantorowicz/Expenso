namespace Expenso.Api.Configuration;

internal interface IAppConfigurationManager
{
    TSettings GetSettings<TSettings>(string sectionName) where TSettings : class;

    void Configure(IServiceCollection serviceCollection);
}