// <copyright file="EntryListResult.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage.Results
{
  /// <summary>
  /// Provides a result set for <see cref="IStorageService.EntryList"/>.
  /// </summary>
  public class EntryListResult
  {
    /// <summary>
    /// Gets or sets the <see cref="IEnumerable{EntryListEntryResult}"/>.
    /// </summary>
    public IEnumerable<EntryListEntryResult>? Entries { get; set; }
  }
}
