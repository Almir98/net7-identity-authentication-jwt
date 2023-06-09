var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Just for read the secrets anywhere in the application
//builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddDbContext<ApiDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAuthentication();

builder.Services.AddIdentityConfiguration();

// Identity 
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApiDbContext>();

//Extenssion method
builder.Services.ConfigureJWT(builder.Configuration);

//Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

//builder.AddDefaultIdentity<User>(options => options.SignIn.RequiredConfirmedAccount = true)
//   .AddEntityFrameworkStores<ApiDbContext>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
