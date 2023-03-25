namespace Authentication.API.Persistance;

public class ApiDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new RoleConfiguration());

        base.OnModelCreating(builder);
    }
}