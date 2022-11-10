// <copyright file="EntryDeleteRequest.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Models.Requests
{
  /// <summary>
  /// Provides a request model for <see cref="Controllers.EntryController.DeleteAsync"/>.
  /// </summary>
  public class EntryDeleteRequest
  {
    /// <summary>
    /// Gets or sets the entry identifier.
    /// </summary>
    public Guid? EntryId { get; set; }
  }
}
