namespace Authentication.API.Configuration;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthenticationService> _authenticationService;
    public ServiceManager(
        IMapper mapper,
        UserManager<User> userManager,
        IConfiguration configuration)
    {
        _authenticationService = new Lazy<IAuthenticationService>(() =>
            new AuthenticationService(mapper, userManager,
                configuration));
    }
    public IAuthenticationService AuthenticationService =>
        _authenticationService.Value;
}