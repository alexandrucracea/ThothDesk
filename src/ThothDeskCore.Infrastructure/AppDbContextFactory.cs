
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace ThothDeskCore.Infrastructure;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // 1) Mediu
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        // 2) Candidati pentru folderul care conține appsettings.json (în funcție de locul din care rulează PMC)
        var cwd = Directory.GetCurrentDirectory(); // ex: C:\Personal Projects\ThothDesk  (root)
        var candidates = new[]
        {
            Path.Combine(cwd, "src", "ThothDeskCore.Api"),
            Path.Combine(cwd, "..", "src", "ThothDeskCore.Api"),
            Path.Combine(cwd, "..", "ThothDeskCore.Api"),
            Path.Combine(cwd, "ThothDeskCore.Api"),
            cwd // fallback (dacă rulezi chiar din proiectul Api)
        };

        // 3) Alege primul care chiar conține appsettings.json
        string? basePath = candidates.FirstOrDefault(p => File.Exists(Path.Combine(p, "appsettings.json")));
        if (basePath is null)
            throw new InvalidOperationException(
                "Nu am găsit appsettings.json. Cautat în: " + string.Join(" | ", candidates));

        // 4) Construiește config
        var cfg = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        // IMPORTANT: cheia trebuie să fie EXACT ca în appsettings (la tine e "SqlServer")
        var connStr = cfg.GetConnectionString("SqlServer");
        if (string.IsNullOrWhiteSpace(connStr))
            throw new InvalidOperationException("Connection string 'SqlServer' nu a fost găsit în config.");

        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connStr)
            .Options;

        return new AppDbContext(opts);
    }
}

