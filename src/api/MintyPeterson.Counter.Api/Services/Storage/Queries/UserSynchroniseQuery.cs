// <copyright file="UserSynchroniseQuery.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage.Queries
{
  /// <summary>
  /// Provides query parameters for <see cref="IStorageService.UserSynchronise"/>.
  /// </summary>
  public class UserSynchroniseQuery
  {
    /// <summary>
    /// Gets or sets the user identifer.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the full name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the e-mail address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DateTimeOffset"/> the user was updated.
    /// </summary>
    public DateTimeOffset UpdatedDateTime { get; set; }
  }
}
