// <copyright file="EntryGetQuery.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage.Queries
{
  /// <summary>
  /// Provides query parameters for <see cref="IStorageService.EntryGet"/>.
  /// </summary>
  public class EntryGetQuery
  {
    /// <summary>
    /// Gets or sets the entry identifer.
    /// </summary>
    public Guid EntryId { get; set; }
  }
}
