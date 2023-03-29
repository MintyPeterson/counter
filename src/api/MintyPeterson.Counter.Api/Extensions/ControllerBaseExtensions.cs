// <copyright file="ControllerBaseExtensions.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Extensions
{
  using System.Text.Json;
  using FluentValidation;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;

  /// <summary>
  /// Provides extension methods for <see cref="ControllerBase"/>.
  /// </summary>
  public static class ControllerBaseExtensions
  {
    /// <summary>
    /// Validates a request against the given validator and authorisation policy.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TLoggerCategoryName">The logger type.</typeparam>
    /// <param name="controller">The controller.</param>
    /// <param name="loggerService">A <see cref="ILogger{TLoggerCategoryName}"/>.</param>
    /// <param name="logIdentifier">The identifier to use when logging.</param>
    /// <param name="request">The request to validate.</param>
    /// <param name="requestValidator">A request validator.</param>
    /// <param name="authorisationService">A <see cref="IAuthorizationService"/>.</param>
    /// <param name="authorisationPolicyName">The authorisation policy name.</param>
    /// <returns>An <see cref="ActionResult"/>. If successful, <see cref="OkResult"/>.</returns>
    public static async Task<ActionResult> ValidateRequestAsync<TRequest, TLoggerCategoryName>(
      this ControllerBase controller,
      ILogger<TLoggerCategoryName> loggerService,
      string logIdentifier,
      TRequest request,
      AbstractValidator<TRequest> requestValidator,
      IAuthorizationService authorisationService,
      string authorisationPolicyName)
    {
      loggerService.LogInformation(
        "{logIdentifier}: Validating model state", logIdentifier);

      if (!controller.ModelState.IsValid)
      {
        loggerService.LogInformation(
          "{logIdentifier}: Model state not valid", logIdentifier);

        if (loggerService.IsEnabled(LogLevel.Debug))
        {
          loggerService.LogDebug(
            "{logIdentifier}: State is {state}",
            logIdentifier,
            JsonSerializer.Serialize(new SerializableError(controller.ModelState)));
        }

        controller.ModelState.Clear();

        controller.ModelState.AddModelError(
          Resources.Strings.Entry,
          Resources.Strings.RequestNotValid);

        return controller.BadRequest(controller.ModelState);
      }

      loggerService.LogInformation(
        "{logIdentifier}: Checking authorisation policy", logIdentifier);

      var requestAuthorisationResult = await authorisationService.AuthorizeAsync(
        controller.User, request, authorisationPolicyName);

      if (!requestAuthorisationResult.Succeeded)
      {
        loggerService.LogInformation(
          "{logIdentifier}: Authorisation failed", logIdentifier);

        return controller.Forbid();
      }

      loggerService.LogInformation(
        "{logIdentifier}: Validating request", logIdentifier);

      var validationResults = requestValidator.Validate(request);

      if (!validationResults.IsValid)
      {
        loggerService.LogInformation(
          "{logIdentifier}: Request not valid", logIdentifier);

        return controller.BadRequest(validationResults.AsModelState());
      }

      return controller.Ok();
    }
  }
}
