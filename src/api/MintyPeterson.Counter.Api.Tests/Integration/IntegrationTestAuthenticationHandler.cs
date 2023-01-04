// <copyright file="IntegrationTestAuthenticationHandler.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Tests.Integration
{
  using System.Security.Claims;
  using System.Text.Encodings.Web;
  using Microsoft.AspNetCore.Authentication;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;

  /// <summary>
  /// Provides an <see cref="AuthenticationHandler{AuthenticationSchemeOptions}"/> for <see cref="IntegrationTest"/>.
  /// </summary>
  internal class IntegrationTestAuthenticationHandler
    : AuthenticationHandler<AuthenticationSchemeOptions>
  {
    /// <summary>
    /// Default value for the authentication scheme property.
    /// </summary>
    public const string AuthenticationScheme = "Integration Test";

    /// <summary>
    /// Default value for the name identifier claim.
    /// </summary>
    private const string DefaultNameIdentifierClaim = "00000000-0000-0000-0000-000000000001";

    /// <summary>
    /// Default value for the name claim.
    /// </summary>
    private const string DefaultNameClaim = "Integration Tester";

    /// <summary>
    /// Default value for the e-mail claim.
    /// </summary>
    private const string DefaultEmailClaim = "integration.tester@drtomcook.uk";

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationTestAuthenticationHandler"/> class.
    /// </summary>
    /// <param name="options">A <see cref="IOptionsMonitor{AuthenticationSchemeOptions}"/>.</param>
    /// <param name="logger">A <see cref="ILoggerFactory"/>.</param>
    /// <param name="encoder">A <see cref="UrlEncoder"/>.</param>
    /// <param name="clock">A <see cref="ISystemClock"/>.</param>
    public IntegrationTestAuthenticationHandler(
      IOptionsMonitor<AuthenticationSchemeOptions> options,
      ILoggerFactory logger,
      UrlEncoder encoder,
      ISystemClock clock)
      : base(options, logger, encoder, clock)
    {
    }

    /// <inheritdoc/>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
      var claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier, DefaultNameIdentifierClaim),
        new Claim(ClaimTypes.Name, DefaultNameClaim),
        new Claim(ClaimTypes.Email, DefaultEmailClaim),
      };

      var identity = new ClaimsIdentity(claims, AuthenticationScheme);
      var principal = new ClaimsPrincipal(identity);
      var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

      var result = AuthenticateResult.Success(ticket);

      return Task.FromResult(result);
    }
  }
}