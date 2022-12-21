// <copyright file="CounterWebApplicationFactory.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Tests.Integration
{
  using Microsoft.AspNetCore;
  using Microsoft.AspNetCore.Authentication;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.Mvc.Testing;
  using Microsoft.AspNetCore.TestHost;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;

  /// <summary>
  /// Provides a factory for bootstrapping <see cref="Api.Program"/>.
  /// </summary>
  public class CounterWebApplicationFactory : WebApplicationFactory<Program>
  {
    /// <summary>
    /// Gets the <see cref="IConfiguration"/>.
    /// </summary>
    public IConfiguration? Configuration { get; private set; }

    /// <inheritdoc/>
    protected override IWebHostBuilder CreateWebHostBuilder()
    {
      return WebHost.CreateDefaultBuilder().UseStartup<Startup>();
    }

    /// <inheritdoc/>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      builder.ConfigureAppConfiguration(
        config =>
        {
          this.Configuration = new ConfigurationBuilder()
            .AddJsonFile(
              "TestSettings.json",
              optional: true,
              reloadOnChange: true)
            .AddJsonFile(
              "TestSettings.Test.json",
              optional: true,
              reloadOnChange: true)
            .Build();

          config.AddConfiguration(this.Configuration);
        });

      builder.ConfigureTestServices(
        services =>
        {
          services
            .AddAuthentication(IntegrationTestAuthenticationHandler.AuthenticationScheme)
            .AddScheme<AuthenticationSchemeOptions, IntegrationTestAuthenticationHandler>(
              IntegrationTestAuthenticationHandler.AuthenticationScheme,
              options =>
              {
              });
        });
    }
  }
}
