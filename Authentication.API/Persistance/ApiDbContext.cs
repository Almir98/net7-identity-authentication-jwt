namespace Authentication.API.Persistance;

public class ApiDbContext : IdentityDbContext<ApplicationUser>
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    { }
}