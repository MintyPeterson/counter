// <copyright file="EntryEditResponse.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Models.Responses
{
  /// <summary>
  /// Provides a response model for <see cref="Controllers.EntryController.EditAsync"/>.
  /// </summary>
  public class EntryEditResponse
  {
    /// <summary>
    /// Gets or sets the entry identifier.
    /// </summary>
    public Guid EntryId { get; set; }
  }
}
