// <copyright file="EntryEditRequestBody.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Models.Requests
{
  /// <summary>
  /// Represents a <see cref="EntryEditRequest"/> body.
  /// </summary>
  public class EntryEditRequestBody
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
  }
}
