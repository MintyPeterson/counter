// <copyright file="EntryController.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Controllers
{
  using System.Text.Json;
  using AutoMapper;
  using FluentValidation;
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
      var validationResult = await this.ValidateRequestAsync(
        "NewAsync",
        request,
        new EntryNewRequestValidator(this.storageService),
        EntryPolicies.NewPolicy);

      if (validationResult is not OkResult)
      {
        return validationResult;
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

    /// <summary>
    /// Deletes an entry.
    /// </summary>
    /// <param name="request">A <see cref="EntryDeleteRequest"/>.</param>
    /// <returns>An <see cref="ActionResult{EntryDeleteResponse}"/>.</returns>
    [HttpDelete("/Entry/{EntryID:Guid?}")]
    public async Task<ActionResult<EntryDeleteResponse>> DeleteAsync(
      [FromRoute]EntryDeleteRequest request)
    {
      var validationResult = await this.ValidateRequestAsync(
        "DeleteAsync",
        request,
        new EntryDeleteRequestValidator(this.storageService),
        EntryPolicies.DeletePolicy);

      if (validationResult is not OkResult)
      {
        return validationResult;
      }

      this.loggerService.LogInformation("DeleteAsync: Deleting entry");

      var query = this.mapperService.Map<EntryDeleteQuery>(request);
      {
        query.DeletedDateTime = DateTimeOffset.Now;
        query.DeletedByUserId = this.User.GetSubjectIdentifier();
      }

      var result = this.storageService.EntryDelete(query);

      if (result == null)
      {
        this.loggerService.LogInformation("DeleteAsync: Delete failed");

        this.ModelState.AddModelError(
          Resources.Strings.Entry,
          Resources.Strings.EntryNotDeleted);

        return this.BadRequest(this.ModelState);
      }

      this.loggerService.LogInformation("DeleteAsync: Building response");

      return this.mapperService.Map<EntryDeleteResponse>(result);
    }

    /// <summary>
    /// Lists entries.
    /// </summary>
    /// <param name="request">A <see cref="EntryListRequest"/>.</param>
    /// <returns>An <see cref="ActionResult{EntryListResponse}"/>.</returns>
    [HttpGet("/Entries")]
    public async Task<ActionResult<EntryListResponse>> ListAsync(
      [FromQuery]EntryListRequest request)
    {
      var validationResult = await this.ValidateRequestAsync(
        "ListAsync",
        request,
        new EntryListRequestValidator(this.storageService),
        EntryPolicies.ListPolicy);

      if (validationResult is not OkResult)
      {
        return validationResult;
      }

      this.loggerService.LogInformation("ListAsync: Listing entries");

      var query = this.mapperService.Map<EntryListQuery>(request);
      {
        query.CreatedByUserId = this.User.GetSubjectIdentifier();
      }

      var result = this.storageService.EntryList(query);

      this.loggerService.LogInformation("ListAsync: Building response");

      var response = new EntryListResponse
      {
        Groups =
        result?.Entries?
          .GroupBy(
            e => e.EntryDate)
          .Select(
            g => new EntryListGroupResponse
            {
              Name = g.Key.ToString("d"),
              Total = g.Sum(e => e.Entry),
              Entries = this.mapperService.Map<IEnumerable<EntryListEntryResponse>>(g),
            }),
      };

      return response;
    }

    /// <summary>
    /// Views an entry.
    /// </summary>
    /// <param name="request">A <see cref="EntryViewRequest"/>.</param>
    /// <returns>An <see cref="ActionResult{EntryViewResponse}"/>.</returns>
    [HttpGet("/Entry/{EntryID:Guid?}")]
    public async Task<ActionResult<EntryViewResponse>> ViewAsync(
      [FromRoute]EntryViewRequest request)
    {
      var validationResult = await this.ValidateRequestAsync(
        "ViewAsync",
        request,
        new EntryViewRequestValidator(this.storageService),
        EntryPolicies.ViewPolicy);

      if (validationResult is not OkResult)
      {
        return validationResult;
      }

      this.loggerService.LogInformation("ViewAsync: Getting entry");

      var result = this.storageService.EntryGet(this.mapperService.Map<EntryGetQuery>(request));

      if (result == null)
      {
        this.loggerService.LogInformation("ViewAsync: Get failed");

        this.ModelState.AddModelError(
          Resources.Strings.Entry,
          Resources.Strings.EntryNotFound);

        return this.BadRequest(this.ModelState);
      }

      this.loggerService.LogInformation("ViewAsync: Building response");

      return this.mapperService.Map<EntryViewResponse>(result);
    }

    /// <summary>
    /// Validates a request against the given validator and authorisation policy.
    /// </summary>
    /// <typeparam name="T">The request type.</typeparam>
    /// <param name="logIdentifier">The identifier to use when logging.</param>
    /// <param name="request">The request to validate.</param>
    /// <param name="requestValidator">A request validator.</param>
    /// <param name="authorisationPolicyName">The authorisation policy name.</param>
    /// <returns>An <see cref="ActionResult"/>. If successful, <see cref="OkResult"/>.</returns>
    public async Task<ActionResult> ValidateRequestAsync<T>(
      string logIdentifier,
      T request,
      AbstractValidator<T> requestValidator,
      string authorisationPolicyName)
    {
      this.loggerService.LogInformation(
        "{logIdentifier}: Validating model state", logIdentifier);

      if (!this.ModelState.IsValid)
      {
        this.loggerService.LogInformation(
          "{logIdentifier}: Model state not valid", logIdentifier);

        if (this.loggerService.IsEnabled(LogLevel.Debug))
        {
          this.loggerService.LogInformation(
            "{logIdentifier}: State is {state}",
            logIdentifier,
            JsonSerializer.Serialize(new SerializableError(this.ModelState)));
        }

        this.ModelState.Clear();

        this.ModelState.AddModelError(
          Resources.Strings.Entry,
          Resources.Strings.RequestNotValid);

        return this.BadRequest(this.ModelState);
      }

      this.loggerService.LogInformation(
        "{logIdentifier}: Checking authorisation policy", logIdentifier);

      var requestAuthorisationResult = await this.authorisationService.AuthorizeAsync(
        this.User, request, authorisationPolicyName);

      if (!requestAuthorisationResult.Succeeded)
      {
        this.loggerService.LogInformation(
          "{logIdentifier}: Authorisation failed", logIdentifier);

        return this.Forbid();
      }

      this.loggerService.LogInformation(
        "{logIdentifier}: Validating request", logIdentifier);

      var validationResults = requestValidator.Validate(request);

      if (!validationResults.IsValid)
      {
        this.loggerService.LogInformation(
          "{logIdentifier}: Request not valid", logIdentifier);

        return this.BadRequest(validationResults.AsModelState());
      }

      return this.Ok();
    }
  }
}
