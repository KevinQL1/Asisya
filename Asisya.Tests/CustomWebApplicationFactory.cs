using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using DotNetEnv;
using System;
using System.IO;
using System.Collections.Generic;

namespace Asisya.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var projectDir = Directory.GetCurrentDirectory();
        var envPath = Path.Combine(projectDir, "..", "..", "..", "..", ".env");
        if (File.Exists(envPath))
        {
            Env.Load(envPath);
        }

        builder.ConfigureAppConfiguration((context, config) =>
        {
            var hostLocal = Environment.GetEnvironmentVariable("DB_HOST_LOCAL") ?? Env.GetString("DB_HOST_LOCAL", "127.0.0.1");
            var port = Environment.GetEnvironmentVariable("PORT_DB") ?? Env.GetString("PORT_DB", "5432");
            var db = Environment.GetEnvironmentVariable("DB_NAME") ?? Env.GetString("DB_NAME", "asisya_catalog");
            var user = Environment.GetEnvironmentVariable("USER") ?? Env.GetString("USER", "asisya_admin");
            var pass = Environment.GetEnvironmentVariable("PASSWORD") ?? Env.GetString("PASSWORD", "4sisy4_Pru3b4");

            var connString = $"Host={hostLocal};Port={port};Database={db};Username={user};Password={pass}";

            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:DefaultConnection", connString }
            });
        });
    }
}