namespace Authentication.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public IdentityController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
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

    [HttpPost("CreateToken")]
    public async Task<IActionResult> CreateToken([FromBody] UserAuthenticationDTO userAuthenticationDto)
    {
        if (!await _authenticationService.ValidateUser(userAuthenticationDto))
            return Unauthorized();

        return Ok(new
        {
            Token = await _authenticationService.CreateToken()
        });
    }
}