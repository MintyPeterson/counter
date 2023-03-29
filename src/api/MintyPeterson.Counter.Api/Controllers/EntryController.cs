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
      var validationResult = await this.ValidateRequestAsync(
        this.loggerService,
        "NewAsync",
        request,
        new EntryNewRequestValidator(this.storageService),
        this.authorisationService,
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

      if (!result.HasSucceeded)
      {
        this.loggerService.LogInformation("NewAsync: Save failed");

        if (this.loggerService.IsEnabled(LogLevel.Debug))
        {
          this.loggerService.LogDebug(
            "NewAsync: Exception is {message}",
            JsonSerializer.Serialize(result.Exception?.Message));
        }

        this.ModelState.AddModelError(
          Resources.Strings.Entry,
          Resources.Strings.EntryNotCreated);

        return this.BadRequest(this.ModelState);
      }

      this.loggerService.LogInformation("NewAsync: Building response");

      return this.mapperService.Map<EntryNewResponse>(result.Result);
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
        this.loggerService,
        "DeleteAsync",
        request,
        new EntryDeleteRequestValidator(this.storageService),
        this.authorisationService,
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

      if (!result.HasSucceeded)
      {
        this.loggerService.LogInformation("DeleteAsync: Delete failed");

        if (this.loggerService.IsEnabled(LogLevel.Debug))
        {
          this.loggerService.LogDebug(
            "DeleteAsync: Exception is {message}",
            JsonSerializer.Serialize(result.Exception?.Message));
        }

        this.ModelState.AddModelError(
          Resources.Strings.Entry,
          Resources.Strings.EntryNotDeleted);

        return this.BadRequest(this.ModelState);
      }

      this.loggerService.LogInformation("DeleteAsync: Building response");

      return this.mapperService.Map<EntryDeleteResponse>(result.Result);
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
        this.loggerService,
        "ListAsync",
        request,
        new EntryListRequestValidator(this.storageService),
        this.authorisationService,
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

      if (!result.HasSucceeded)
      {
        this.loggerService.LogInformation("ListAsync: List entries failed");

        if (this.loggerService.IsEnabled(LogLevel.Debug))
        {
          this.loggerService.LogDebug(
            "ListAsync: Exception is {message}",
            JsonSerializer.Serialize(result.Exception?.Message));
        }
      }

      this.loggerService.LogInformation("ListAsync: Building response");

      var response = new EntryListResponse();

      if (result.HasSucceeded)
      {
        response.Groups =
          result.Result!.Entries?
            .GroupBy(
              e => e.EntryDate)
            .Select(
              g => new EntryListGroupResponse
              {
                Name = g.Key.ToString("d"),
                Total = g.Sum(e => e.Entry),
                IsEstimate = g.Any(e => e.IsEstimate),
                Entries = this.mapperService.Map<IEnumerable<EntryListEntryResponse>>(g),
              });
      }

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
        this.loggerService,
        "ViewAsync",
        request,
        new EntryViewRequestValidator(this.storageService),
        this.authorisationService,
        EntryPolicies.ViewPolicy);

      if (validationResult is not OkResult)
      {
        return validationResult;
      }

      this.loggerService.LogInformation("ViewAsync: Getting entry");

      var result = this.storageService.EntryGet(this.mapperService.Map<EntryGetQuery>(request));

      if (!result.HasSucceeded)
      {
        this.loggerService.LogInformation("ViewAsync: Get failed");

        if (this.loggerService.IsEnabled(LogLevel.Debug))
        {
          this.loggerService.LogDebug(
            "ViewAsync: Exception is {message}",
            JsonSerializer.Serialize(result.Exception?.Message));
        }

        this.ModelState.AddModelError(
          Resources.Strings.Entry,
          Resources.Strings.EntryNotFound);

        return this.BadRequest(this.ModelState);
      }

      this.loggerService.LogInformation("ViewAsync: Building response");

      return this.mapperService.Map<EntryViewResponse>(result.Result);
    }

    /// <summary>
    /// Updates an entry.
    /// </summary>
    /// <param name="request">A <see cref="EntryEditRequest"/>.</param>
    /// <returns>An <see cref="ActionResult{EntryEditResponse}"/>.</returns>
    [HttpPut("/Entry/{EntryID:Guid?}")]
    public async Task<ActionResult<EntryEditResponse>> EditAsync(EntryEditRequest request)
    {
      var validationResult = await this.ValidateRequestAsync(
        this.loggerService,
        "EditAsync",
        request,
        new EntryEditRequestValidator(this.storageService),
        this.authorisationService,
        EntryPolicies.EditPolicy);

      if (validationResult is not OkResult)
      {
        return validationResult;
      }

      this.loggerService.LogInformation("EditAsync: Updating entry");

      var query = this.mapperService.Map<EntryEditQuery>(request.Body);
      {
        query.EntryId = request.EntryId!.Value;
        query.UpdatedDateTime = DateTimeOffset.Now;
        query.UpdatedByUserId = this.User.GetSubjectIdentifier();
      }

      var result = this.storageService.EntryEdit(query);

      if (!result.HasSucceeded)
      {
        this.loggerService.LogInformation("EditAsync: Update failed");

        if (this.loggerService.IsEnabled(LogLevel.Debug))
        {
          this.loggerService.LogDebug(
            "EditAsync: Exception is {message}",
            JsonSerializer.Serialize(result.Exception?.Message));
        }

        this.ModelState.AddModelError(
          Resources.Strings.Entry,
          Resources.Strings.EntryNotUpdated);

        return this.BadRequest(this.ModelState);
      }

      this.loggerService.LogInformation("EditAsync: Building response");

      return this.mapperService.Map<EntryEditResponse>(result.Result);
    }
  }
}
