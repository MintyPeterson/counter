// <copyright file="EntryListGroupResponse.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Models.Responses
{
  /// <summary>
  /// Represents an group of <see cref="EntryListEntryResponse"/> objects in a <see cref="EntryListResponse"/>.
  /// </summary>
  public class EntryListGroupResponse
  {
    /// <summary>
    /// Gets or sets the group name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the sum of <see cref="EntryListEntryResponse.Entry"/> in <see cref="Entries"/>.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the total is an estimate or not.
    /// </summary>
    public bool IsEstimate { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="IEnumerable{EntryListEntryResponse}"/>.
    /// </summary>
    public IEnumerable<EntryListEntryResponse>? Entries { get; set; }
  }
}
