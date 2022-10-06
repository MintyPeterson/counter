// <copyright file="EntryNewQuery.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage.Queries
{
  /// <summary>
  /// Provides query parameters for <see cref="IStorageService.EntryNew"/>.
  /// </summary>
  public class EntryNewQuery
  {
    /// <summary>
    /// Gets or sets the <see cref="DateTimeOffset"/> the entry was created.
    /// </summary>
    public DateTimeOffset CreatedDateTime { get; set; }

    /// <summary>
    /// Gets or sets the identifer of the user who created the entry.
    /// </summary>
    public string? CreatedByUserId { get; set; }

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
