// <copyright file="EntryEditResult.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage.Results
{
  /// <summary>
  /// Provides a result set for <see cref="IStorageService.EntryEdit"/>.
  /// </summary>
  public class EntryEditResult
  {
    /// <summary>
    /// Gets or sets the entry identifier.
    /// </summary>
    public Guid EntryId { get; set; }
  }
}
