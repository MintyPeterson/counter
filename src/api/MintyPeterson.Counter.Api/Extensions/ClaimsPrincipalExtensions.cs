// <copyright file="ClaimsPrincipalExtensions.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Extensions
{
  using System.Security.Claims;

  /// <summary>
  /// Provides extension methods for <see cref="ClaimsPrincipal"/>.
  /// </summary>
  public static class ClaimsPrincipalExtensions
  {
    /// <summary>
    /// Gets the subject identifier.
    /// </summary>
    /// <param name="principal">A <see cref="ClaimsPrincipal"/>.</param>
    /// <returns>The subject identifier.</returns>
    public static string? GetSubjectIdentifier(this ClaimsPrincipal principal)
    {
      return principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    /// <summary>
    /// Gets the full name.
    /// </summary>
    /// <param name="principal">A <see cref="ClaimsPrincipal"/>.</param>
    /// <returns>The full name.</returns>
    public static string? GetName(this ClaimsPrincipal principal)
    {
      return principal.FindFirstValue(ClaimTypes.Name);
    }

    /// <summary>
    /// Gets the e-mail address.
    /// </summary>
    /// <param name="principal">A <see cref="ClaimsPrincipal"/>.</param>
    /// <returns>The e-mail address.</returns>
    public static string? GetEmail(this ClaimsPrincipal principal)
    {
      return principal.FindFirstValue(ClaimTypes.Email);
    }
  }
}
