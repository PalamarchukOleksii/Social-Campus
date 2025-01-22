namespace Presentation.Utils
{
    public static class LogDetails
    {
        public static void LogEnvironmentDetails(ILogger logger, IConfiguration configuration)
        {
            bool isRunningInDocker = configuration["DOTNET_RUNNING_IN_CONTAINER"] == "true";
            string activeConnectionStringKey = isRunningInDocker ? "DockerConnection" : "LocalConnection";
            string? activeConnectionString = configuration.GetConnectionString(activeConnectionStringKey);

            logger.LogInformation("DOTNET_RUNNING_IN_CONTAINER: {InContainer}", isRunningInDocker);
            logger.LogInformation("Using Connection String: {ConnectionStringKey} = {ConnectionString}", activeConnectionStringKey, activeConnectionString);
        }
    }
}
