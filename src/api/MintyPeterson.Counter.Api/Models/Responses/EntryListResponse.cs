// <copyright file="EntryListResponse.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Models.Responses
{
  /// <summary>
  /// Provides a response model for <see cref="Controllers.EntryController.ListAsync"/>.
  /// </summary>
  public class EntryListResponse
  {
    /// <summary>
    /// Gets or sets the <see cref="IEnumerable{EntryListEntryResponse}"/>.
    /// </summary>
    public IEnumerable<EntryListEntryResponse>? Entries { get; set; }
  }
}
