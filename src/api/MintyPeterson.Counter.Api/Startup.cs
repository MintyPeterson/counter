﻿// <copyright file="Startup.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api
{
  using System.Globalization;
  using Microsoft.AspNetCore.Authentication.JwtBearer;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Localization;
  using Microsoft.AspNetCore.Mvc;
  using MintyPeterson.Counter.Api.Exceptions;
  using MintyPeterson.Counter.Api.Filters;
  using MintyPeterson.Counter.Api.Policies;
  using MintyPeterson.Counter.Api.Resources;
  using MintyPeterson.Counter.Api.Services.Storage;

  /// <summary>
  /// Provides the startup type used by the <see cref="IWebHostBuilder"/>.
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// Configures the services.
    /// </summary>
    /// <param name="services">A <see cref="IServiceCollection"/>.</param>
    /// <remarks>
    /// This is called automatically to configure the services.
    /// </remarks>
    public void ConfigureServices(IServiceCollection services)
    {
      var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

      if (string.IsNullOrWhiteSpace(environment))
      {
        environment = "Development";
      }

      var configuration =
        new ConfigurationBuilder()
          .AddJsonFile(
            "AppSettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile(
            $"AppSettings.{environment}.json",
            optional: true,
            reloadOnChange: true)
          .Build();

      services.AddLocalization();
      services.AddAutoMapper(typeof(Startup));

      services.AddTransient<IStorageService>(
        options =>
        {
          var connectionString = configuration.GetConnectionString("DapperStorageService");

          if (string.IsNullOrWhiteSpace(connectionString))
          {
            throw new ConfigurationMissingException(
              "The Dapper storage service connection string is missing.");
          }

          return new DapperStorageService(connectionString);
        });

      // Add handlers for policy-based authorisation.
      var handlers = typeof(Startup).Assembly.GetTypes().Where(
        t => t.IsClass && typeof(IAuthorizationHandler).IsAssignableFrom(t));

      foreach (var handler in handlers)
      {
        services.AddTransient(typeof(IAuthorizationHandler), handler);
      }

      services.AddAuthorization(
        options =>
        {
          EntryPolicies.AddPolicies(options);
        });

      services.AddControllers(
        options =>
        {
          options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
            (a, b) => Strings.PropertyValueInvalid);

          options.Filters.Add<UserSynchroniseActionFilter>();
        });

      services.Configure<ApiBehaviorOptions>(
        options =>
        {
          options.SuppressModelStateInvalidFilter = true;
          options.SuppressInferBindingSourcesForParameters = true;
        });

      services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(
          options =>
          {
            options.Authority = configuration.GetValue<string>("IdentityServerAddress");
            options.Audience = "counter_api";

            options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
          });

      services.AddCors(
        options =>
        {
          options.AddDefaultPolicy(
            policy =>
            {
              policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
    }

    /// <summary>
    /// Configures the application.
    /// </summary>
    /// <param name="application">A <see cref="IApplicationBuilder"/>.</param>
    /// <param name="environment">A <see cref="IWebHostEnvironment"/>.</param>
    /// <remarks>
    /// This is called automatically to create the request processing pipeline.
    /// </remarks>
    public void Configure(
      IApplicationBuilder application, IWebHostEnvironment environment)
    {
      if (environment.IsDevelopment())
      {
        application.UseDeveloperExceptionPage();
      }

      var supportedCultures =
        new CultureInfo[]
        {
          new CultureInfo("en-GB"),
        };

      application.UseRequestLocalization(
        new RequestLocalizationOptions
        {
          SupportedCultures = supportedCultures,
          SupportedUICultures = supportedCultures,

          DefaultRequestCulture = new RequestCulture("en-GB"),
        });

      application.UseRouting();
      application.UseCors();
      application.UseAuthentication();
      application.UseAuthorization();

      application.UseEndpoints(
        endpoints =>
        {
          endpoints.MapControllers();
        });
    }
  }
}
