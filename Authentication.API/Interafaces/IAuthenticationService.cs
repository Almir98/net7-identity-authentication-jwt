namespace Authentication.API.Interafaces;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserRegistrationDTO userForRegistration);
    Task<UserRegistrationDTO> FindByEmail(UserRegistrationDTO userRegistrationDto);
    Task<bool> ValidateUser(UserAuthenticationDTO userForAuth);
    Task<string> CreateToken();
}