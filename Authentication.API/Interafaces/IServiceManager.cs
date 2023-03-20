namespace Authentication.API.Interafaces;

public interface IServiceManager
{
    IAuthenticationService AuthenticationService { get; }
}