// <copyright file="UserSynchroniseActionFilter.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Filters
{
  using AutoMapper;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.Filters;
  using MintyPeterson.Counter.Api.Extensions;
  using MintyPeterson.Counter.Api.Services.Storage;
  using MintyPeterson.Counter.Api.Services.Storage.Queries;

  /// <summary>
  /// Ensure the user details are sychronised with the identity provider.
  /// </summary>
  public class UserSynchroniseActionFilter : IActionFilter
  {
    /// <summary>
    /// Stores the <see cref="IStorageService"/> dependency.
    /// </summary>
    private readonly IStorageService storageService;

    /// <summary>
    /// Stores the <see cref="ILogger"/> dependency.
    /// </summary>
    private readonly ILogger<UserSynchroniseActionFilter> loggerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserSynchroniseActionFilter"/> class.
    /// </summary>
    /// <param name="storageService">An <see cref="IStorageService"/>.</param>
    /// <param name="loggerService">An <see cref="ILogger"/>.</param>
    public UserSynchroniseActionFilter(
      IStorageService storageService,
      ILogger<UserSynchroniseActionFilter> loggerService)
    {
      this.storageService = storageService;
      this.loggerService = loggerService;
    }

    /// <inheritdoc/>
    public void OnActionExecuting(ActionExecutingContext context)
    {
      this.loggerService.LogInformation("OnActionExecuting: Synchronising user details");

      var user = context.HttpContext.User;

      if (user.Identity == null)
      {
        this.loggerService.LogInformation("OnActionExecuting: User identity does not exist");

        return;
      }

      if (!user.Identity.IsAuthenticated)
      {
        this.loggerService.LogInformation("OnActionExecuting: User is not authenticated");

        return;
      }

      var userSynchroniseResult = this.storageService.UserSynchronise(
        new UserSynchroniseQuery
        {
          UserID = user.GetSubjectIdentifier(),
          Name = user.GetName(),
          Email = user.GetEmail(),
          UpdatedDateTime = DateTimeOffset.Now,
        });

      if (userSynchroniseResult == null)
      {
        this.loggerService.LogInformation("OnActionExecuting: Synchronise user failed");

        context.ModelState.AddModelError(
          Resources.Strings.User,
          Resources.Strings.UserNotSynchronised);

        context.Result = new BadRequestObjectResult(context.ModelState);
      }
    }

    /// <inheritdoc/>
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
  }
}
