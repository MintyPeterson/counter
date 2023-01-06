// <copyright file="EntryDeleteQuery.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage.Queries
{
  /// <summary>
  /// Provides query parameters for <see cref="IStorageService.EntryDelete"/>.
  /// </summary>
  public class EntryDeleteQuery
  {
    /// <summary>
    /// Gets or sets the entry identifer.
    /// </summary>
    public Guid EntryId { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DateTimeOffset"/> the entry was deleted.
    /// </summary>
    public DateTimeOffset DeletedDateTime { get; set; }

    /// <summary>
    /// Gets or sets the identifer of the user who deleted the entry.
    /// </summary>
    public string? DeletedByUserId { get; set; }
  }
}
