namespace Mvc8.Services;

// Add services to the container.
public class ExternalRegristrationService : IRegristrationService
{
    public string Register(string name)
    {
        return $"Hello {name}, your external registration is successful!";
    }
}
