namespace Authentication.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthenticationController(IAuthenticationService authenticationService, UserManager<IdentityUser> userManager)
    {
        _authenticationService = authenticationService;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDTO userRegistrationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = _userManager.FindByEmailAsync(userRegistrationDto.Email);

        if (existingUser != null)
            return BadRequest(new IdentityResult());

        await _authenticationService.RegisterUser(userRegistrationDto);

        return Ok(201);
    }
}