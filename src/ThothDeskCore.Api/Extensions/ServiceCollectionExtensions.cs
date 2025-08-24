using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ThothDeskCore.Infrastructure;

namespace ThothDeskCore.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("SqlServer") ??
                               throw new InvalidOperationException("Missing connection string 'SqlServer'");

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        return services;
    }

    public static IServiceCollection AddIdentityAndJwt(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        }).AddRoles<IdentityRole>()
          .AddEntityFrameworkStores<AppDbContext>()
          .AddSignInManager();

        var jwt = config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt["Issuer"],
                    ValidAudience = jwt["Audience"],
                    IssuerSigningKey = key
                };
            });

        services.AddAuthorization();
        return services;
    }
}

