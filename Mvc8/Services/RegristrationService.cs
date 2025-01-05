namespace Mvc8.Services;

// Add services to the container.
public class RegristrationService : IRegristrationService
{
    public string Register(string name)
    {
        return $"Hello {name}, your registration is successful!";
    }
}
