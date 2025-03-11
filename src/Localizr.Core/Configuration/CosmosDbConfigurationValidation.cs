using Microsoft.Extensions.Options;

namespace Localizr.Core.Configuration;

public class CosmosDbConfigurationValidation : IValidateOptions<CosmosDbConfiguration>
{
    public ValidateOptionsResult Validate(string? name, CosmosDbConfiguration options)
    {

        var errorList = new List<string>();
        if (string.IsNullOrWhiteSpace(options.DatabaseName))
        {
            errorList.Add($"Reviews service misconfiguration! Missing configuration value for '{CosmosDbConfiguration.DefaultSectionName}.{nameof(CosmosDbConfiguration.DatabaseName)}'.");
        }
        if (string.IsNullOrWhiteSpace(options.ProjectsContainer))
        {
            errorList.Add($"Reviews service misconfiguration! Missing configuration value for '{CosmosDbConfiguration.DefaultSectionName}.{nameof(CosmosDbConfiguration.ProjectsContainer)}'.");
        }

        if (errorList.Any())
        {
            return ValidateOptionsResult.Fail(errorList);
        }

        return ValidateOptionsResult.Success;
    }
}