// <copyright file="EntryEditRequest.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Models.Requests
{
  using Microsoft.AspNetCore.Mvc;

  /// <summary>
  /// Provides a request model for <see cref="Controllers.EntryController.EditAsync"/>.
  /// </summary>
  public class EntryEditRequest
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="EntryEditRequest"/> class.
    /// </summary>
    public EntryEditRequest()
    {
      this.Body = new EntryEditRequestBody();
    }

    /// <summary>
    /// Gets or sets the entry identifier.
    /// </summary>
    [FromRoute]
    public Guid? EntryId { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="EntryEditRequestBody"/>.
    /// </summary>
    [FromBody]
    public EntryEditRequestBody Body { get; set; }
  }
}
