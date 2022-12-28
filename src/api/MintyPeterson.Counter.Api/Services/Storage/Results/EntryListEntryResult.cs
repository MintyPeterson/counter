﻿// <copyright file="EntryListEntryResult.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage.Results
{
  /// <summary>
  /// Represents an entry in a <see cref="EntryListResult"/>.
  /// </summary>
  public class EntryListEntryResult
  {
    /// <summary>
    /// Gets or sets the entry identifier.
    /// </summary>
    public Guid EntryId { get; set; }

    /// <summary>
    /// Gets or sets the entry date.
    /// </summary>
    /// <remarks>Only the date part is retained.</remarks>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Gets or sets the entry.
    /// </summary>
    public decimal Entry { get; set; }

    /// <summary>
    /// Gets or sets the notes.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entry is an estimate or not.
    /// </summary>
    public bool IsEstimate { get; set; }
  }
}
