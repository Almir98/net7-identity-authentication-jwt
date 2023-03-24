using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authentication.API.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly JwtConfig _jwtConfig;

    public AuthenticationService(IMapper mapper, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }

    private string CreateToken(IdentityUser user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(

            issuer: _configuration.GetValue<string>("Token:Issuer"),
            audience: _configuration.GetValue<string>("Token:Audience"),
            claims: new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString())
            },
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: signingCredentials);










        return null;
    }

    public async Task<IdentityResult> RegisterUser(UserRegistrationDTO userForRegistration)
    {
        var user = _mapper.Map<ApplicationUser>(userForRegistration);

        var result = await _userManager.CreateAsync(user, userForRegistration.Password);
        
        if (result.Succeeded)
            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
        
        return result;
    }

    public async Task<UserRegistrationDTO> FindByEmail(UserRegistrationDTO userRegistrationDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(userRegistrationDto.Email);

        return userRegistrationDto;
    }
}