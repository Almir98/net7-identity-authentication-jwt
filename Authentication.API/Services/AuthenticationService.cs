namespace Authentication.API.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    private ApplicationUser? _user;

    public AuthenticationService(
        IMapper mapper,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration
        )
    {
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<IdentityResult> RegisterUser(UserRegistrationDTO userForRegistration)
    {
        var user = _mapper.Map<ApplicationUser>(userForRegistration);
        var result = await _userManager.CreateAsync(user, userForRegistration.Password);

        if (result.Succeeded)
            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);

        return result;
    }

    public async Task<bool> ValidateUser(UserAuthenticationDTO userForAuth)
    {
        _user = await _userManager.FindByNameAsync(userForAuth.UserName);

        var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password));

        return result;
    }

    public async Task<UserRegistrationDTO> FindByEmail(UserRegistrationDTO userRegistrationDto)
    {
        var user = _mapper.Map<ApplicationUser>(userRegistrationDto);

        var existingUser = await _userManager.FindByEmailAsync(user.Email);

        return _mapper.Map<UserRegistrationDTO>(existingUser);
    }

    // Create Token method
    public async Task<string> CreateToken()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    // Returns out secret key as a byte array
    private SigningCredentials GetSigningCredentials()
    {
        var conf = _configuration.GetSection("JwtSettings:Secret").ToString();

        var key = Encoding.UTF8.GetBytes(conf);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    // Creates a list of claims with the user
    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim("UserId", _user.Id),
            new Claim("FirstName", _user.FirstName),
            new Claim("LastName", _user.LastName),
            new Claim("Username", _user.UserName),
            new Claim("Email", _user.Email),
            //new Claim(ClaimTypes.Name, _user.UserName),
        };
        var roles = await _userManager.GetRolesAsync(_user);

        foreach (var role in roles)
        {
            claims.Add(new Claim("Role", role));
        }
        return claims;
    }

    // Creates an object of the JwtSecurityToken type with all of the required options
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var tokenOptions = new JwtSecurityToken
        (
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }
}
