// <copyright file="EntryGetResult.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage.Results
{
  /// <summary>
  /// Provides a result set for <see cref="IStorageService.EntryGet"/>.
  /// </summary>
  public class EntryGetResult
  {
    /// <summary>
    /// Gets or sets the entry identifier.
    /// </summary>
    public Guid EntryId { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DateTimeOffset"/> the entry was created.
    /// </summary>
    public DateTimeOffset CreatedDateTime { get; set; }

    /// <summary>
    /// Gets or sets the identifer of the user who created the entry.
    /// </summary>
    public string? CreatedByUserId { get; set; }

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
