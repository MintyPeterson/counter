// <copyright file="EntryEditQuery.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage.Queries
{
  /// <summary>
  /// Provides query parameters for <see cref="IStorageService.EntryEdit"/>.
  /// </summary>
  public class EntryEditQuery
  {
    /// <summary>
    /// Gets or sets the entry identifer.
    /// </summary>
    public Guid EntryID { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DateTimeOffset"/> the entry was updated.
    /// </summary>
    public DateTimeOffset UpdatedDateTime { get; set; }

    /// <summary>
    /// Gets or sets the identifer of the user who updated the entry.
    /// </summary>
    public string? UpdatedByUserId { get; set; }

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
  }
}
