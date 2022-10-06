// <copyright file="EntryNewHandler.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Policies
{
  using AutoMapper;
  using Microsoft.AspNetCore.Authorization;
  using MintyPeterson.Counter.Api.Models.Requests;
  using MintyPeterson.Counter.Api.Services.Storage;

  /// <summary>
  /// Provides a <see cref="AuthorizationHandler{EntryNewRequirements}"/>.
  /// </summary>
  public class EntryNewHandler : AuthorizationHandler<EntryNewRequirements, EntryNewRequest>
  {
    /// <summary>
    /// Stores the <see cref="IStorageService"/> dependency.
    /// </summary>
    private readonly IStorageService storageService;

    /// <summary>
    /// Stores the <see cref="IMapper"/> dependency.
    /// </summary>
    private readonly IMapper mapperService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryNewHandler"/> class.
    /// </summary>
    /// <param name="storageService">An <see cref="IStorageService"/>.</param>
    /// <param name="mapperService">An <see cref="IMapper"/>.</param>
    public EntryNewHandler(IStorageService storageService, IMapper mapperService)
    {
      this.storageService = storageService;
      this.mapperService = mapperService;
    }

    /// <inheritdoc/>
    protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      EntryNewRequirements requirement,
      EntryNewRequest resource)
    {
      if (!context.HasSucceeded)
      {
        context.Succeed(requirement);
      }

      return Task.CompletedTask;
    }
  }
}
