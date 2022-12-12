﻿// <copyright file="EntryDeleteHandler.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Policies
{
  using AutoMapper;
  using Microsoft.AspNetCore.Authorization;
  using MintyPeterson.Counter.Api.Extensions;
  using MintyPeterson.Counter.Api.Models.Requests;
  using MintyPeterson.Counter.Api.Services.Storage;
  using MintyPeterson.Counter.Api.Services.Storage.Queries;

  /// <summary>
  /// Provides a <see cref="AuthorizationHandler{EntryDeleteRequirements}"/>.
  /// </summary>
  public class EntryDeleteHandler
    : AuthorizationHandler<EntryDeleteRequirements, EntryDeleteRequest>
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
    /// Initializes a new instance of the <see cref="EntryDeleteHandler"/> class.
    /// </summary>
    /// <param name="storageService">An <see cref="IStorageService"/>.</param>
    /// <param name="mapperService">An <see cref="IMapper"/>.</param>
    public EntryDeleteHandler(IStorageService storageService, IMapper mapperService)
    {
      this.storageService = storageService;
      this.mapperService = mapperService;
    }

    /// <inheritdoc/>
    protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      EntryDeleteRequirements requirement,
      EntryDeleteRequest resource)
    {
      if (!context.HasSucceeded)
      {
        var entryGetQuery = this.mapperService.Map<EntryGetQuery>(resource);
        var entryGetResult = this.storageService.EntryGet(entryGetQuery);

        if (entryGetResult == null)
        {
          // Entry does not exist.
          context.Succeed(requirement);
        }
        else
        {
          if (context.User?.GetSubjectIdentifier() == entryGetResult.CreatedByUserId)
          {
            // User created the entry.
            context.Succeed(requirement);
          }
        }
      }

      return Task.CompletedTask;
    }
  }
}