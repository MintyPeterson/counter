// <copyright file="EntryController.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Controllers
{
  using System.Text.Json;
  using AutoMapper;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using MintyPeterson.Counter.Api.Extensions;
  using MintyPeterson.Counter.Api.Models.Requests;
  using MintyPeterson.Counter.Api.Models.Responses;
  using MintyPeterson.Counter.Api.Policies;
  using MintyPeterson.Counter.Api.Services.Storage;
  using MintyPeterson.Counter.Api.Services.Storage.Queries;
  using MintyPeterson.Counter.Api.Validators;

  /// <summary>
  /// Provides actions for entry routes.
  /// </summary>
  [ApiController]
  [Authorize]
  public class EntryController : ControllerBase
  {
    /// <summary>
    /// Stores the <see cref="IAuthorizationService"/> dependency.
    /// </summary>
    private readonly IAuthorizationService authorisationService;

    /// <summary>
    /// Stores the <see cref="IStorageService"/> dependency.
    /// </summary>
    private readonly IStorageService storageService;

    /// <summary>
    /// Stores the <see cref="IMapper"/> dependency.
    /// </summary>
    private readonly IMapper mapperService;

    /// <summary>
    /// Stores the <see cref="ILogger"/> dependency.
    /// </summary>
    private readonly ILogger<EntryController> loggerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryController"/> class.
    /// </summary>
    /// <param name="authorisationService">An <see cref="IAuthorizationService"/>.</param>
    /// <param name="storageService">An <see cref="IStorageService"/>.</param>
    /// <param name="mapperService">An <see cref="IMapper"/>.</param>
    /// <param name="loggerService">An <see cref="ILogger"/>.</param>
    public EntryController(
      IAuthorizationService authorisationService,
      IStorageService storageService,
      IMapper mapperService,
      ILogger<EntryController> loggerService)
    {
      this.authorisationService = authorisationService;
      this.storageService = storageService;
      this.mapperService = mapperService;
      this.loggerService = loggerService;
    }

    /// <summary>
    /// Creates a new entry.
    /// </summary>
    /// <param name="request">A <see cref="EntryNewRequest"/>.</param>
    /// <returns>An <see cref="ActionResult{EntryNewResponse}"/>.</returns>
    [HttpPost("/Entry")]
    public async Task<ActionResult<EntryNewResponse>> NewAsync([FromBody]EntryNewRequest request)
    {
      this.loggerService.LogInformation("NewAsync: Validating model state");

      if (!this.ModelState.IsValid)
      {
        this.loggerService.LogInformation("NewAsync: Model state not valid");

        if (this.loggerService.IsEnabled(LogLevel.Debug))
        {
          this.loggerService.LogInformation(
            "NewAsync: State is {state}",
            JsonSerializer.Serialize(new SerializableError(this.ModelState)));
        }

        this.ModelState.Clear();

        this.ModelState.AddModelError(
          Resources.Strings.Entry,
          Resources.Strings.RequestNotValid);

        return this.BadRequest(this.ModelState);
      }

      this.loggerService.LogInformation("NewAsync: Checking authorisation policy");

      var requestAuthorisationResult = await this.authorisationService.AuthorizeAsync(
        this.User, request, EntryPolicies.NewPolicy);

      if (!requestAuthorisationResult.Succeeded)
      {
        this.loggerService.LogInformation("NewAsync: Authorisation failed");

        return this.Forbid();
      }

      this.loggerService.LogInformation("NewAsync: Validating request");

      var requestValidator = new EntryNewRequestValidator(this.storageService).Validate(request);

      if (!requestValidator.IsValid)
      {
        this.loggerService.LogInformation("NewAsync: Request not valid");

        return this.BadRequest(requestValidator.AsModelState());
      }

      this.loggerService.LogInformation("NewAsync: Saving entry");

      var query = this.mapperService.Map<EntryNewQuery>(request);
      {
        query.CreatedDateTime = DateTimeOffset.Now;
        query.CreatedByUserId = this.User.GetSubjectIdentifier();
      }

      var result = this.storageService.EntryNew(query);

      if (result == null)
      {
        this.loggerService.LogInformation("NewAsync: Save failed");

        this.ModelState.AddModelError(
          Resources.Strings.Entry,
          Resources.Strings.EntryNotCreated);

        return this.BadRequest(this.ModelState);
      }

      this.loggerService.LogInformation("NewAsync: Building response");

      return this.mapperService.Map<EntryNewResponse>(result);
    }
  }
}
