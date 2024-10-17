using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ParfumeExpressApi.Data;
using ParfumeExpressApi.Interfaces;
using ParfumeExpressApi.Models;
using ParfumeExpressApi.Repositories;
using ParfumeExpressApi.Helper;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter 'Bearer' [space] and your token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services and JWT authentication
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // For testing purposes (use HTTPS in production)
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    options.MapInboundClaims = false; // Disable default claim mapping
});


builder.Services.AddAuthorization();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUserManagmentRepository, UserManagmentRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();


// Add CORS policy to allow specific origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("https://localhost:7283") // Replace with your MVC app's URL
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

var app = builder.Build();

// Use a scope to access services like RoleManager and UserManager
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    // Call the RoleSeeder to seed roles and admin user
    await RoleSeeder.SeedRolesAndAdminUser(roleManager, userManager);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/auth/register", async (UserManager<IdentityUser> userManager, [FromBody] RegisterDto registerDto) =>
{
    if (string.IsNullOrEmpty(registerDto.Username) || string.IsNullOrEmpty(registerDto.Email) || string.IsNullOrEmpty(registerDto.Password))
    {
        return Results.BadRequest(new { error = "Username, email, and password are required." });
    }

    var existingUser = await userManager.FindByNameAsync(registerDto.Username);
    if (existingUser != null)
    {
        return Results.BadRequest(new { error = "Username is already taken." });
    }

    var user = new IdentityUser
    {
        UserName = registerDto.Username,
        Email = registerDto.Email
    };

    var result = await userManager.CreateAsync(user, registerDto.Password);
    if (result.Succeeded)
    {
        return Results.Ok(new { message = "Registration successful" });
    }
    else
    {
        return Results.BadRequest(new { error = "Registration failed", errors = result.Errors });
    }
});

app.MapPost("/api/auth/login", async (UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, [FromBody] LoginDto loginDto, JwtTokenService jwtTokenService) =>
{
    var user = await userManager.FindByEmailAsync(loginDto.Email);
    if (user == null)
    {
        return Results.BadRequest(new { error = "Invalid email or password." });
    }

    var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
    if (!result.Succeeded)
    {
        return Results.BadRequest(new { error = "Invalid email or password." });
    }

    // Generate JWT token
    var token = jwtTokenService.GenerateJwtToken(user);
    return Results.Ok(new { token });
});

app.MapControllers();

app.Run();
