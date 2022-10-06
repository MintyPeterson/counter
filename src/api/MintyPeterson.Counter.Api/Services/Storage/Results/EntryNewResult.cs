// <copyright file="EntryNewResult.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage.Results
{
  /// <summary>
  /// Provides a result set for <see cref="IStorageService.EntryNew"/>.
  /// </summary>
  public class EntryNewResult
  {
    /// <summary>
    /// Gets or sets the entry identifier.
    /// </summary>
    public Guid EntryId { get; set; }
  }
}
