// <copyright file="EntryNewRequest.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Models.Requests
{
  /// <summary>
  /// Provides a request model for <see cref="Controllers.EntryController.NewAsync"/>.
  /// </summary>
  public class EntryNewRequest
  {
    /// <summary>
    /// Gets or sets the entry date.
    /// </summary>
    /// <remarks>Only the date part is retained.</remarks>
    public DateTime? EntryDate { get; set; }

    /// <summary>
    /// Gets or sets the entry.
    /// </summary>
    public decimal? Entry { get; set; }

    /// <summary>
    /// Gets or sets the notes.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the entry is an estimate or not.
    /// </summary>
    public bool? IsEstimate { get; set; }
  }
}
