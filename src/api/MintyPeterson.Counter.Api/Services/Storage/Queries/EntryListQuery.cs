// <copyright file="EntryListQuery.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage.Queries
{
  /// <summary>
  /// Provides query parameters for <see cref="IStorageService.EntryList"/>.
  /// </summary>
  public class EntryListQuery
  {
    /// <summary>
    /// Gets or sets the identifer of the user who created the entry.
    /// </summary>
    public string? CreatedByUserId { get; set; }
  }
}
