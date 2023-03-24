namespace Authentication.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly JwtConfig _jwtConfig;

    public IdentityController(IAuthenticationService authenticationService, JwtConfig jwtConfig)
    {
        _authenticationService = authenticationService;
        _jwtConfig = jwtConfig;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDTO userRegistrationDto)
    {
        var existingUser = await _authenticationService.FindByEmail(userRegistrationDto);

        if (existingUser != null)
            return BadRequest();

        await _authenticationService.RegisterUser(userRegistrationDto);

        return Ok();
    }
}