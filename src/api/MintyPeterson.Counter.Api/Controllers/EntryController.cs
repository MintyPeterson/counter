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

    /// <summary>
    /// Deletes an entry.
    /// </summary>
    /// <param name="request">A <see cref="EntryDeleteRequest"/>.</param>
    /// <returns>An <see cref="ActionResult{EntryDeleteResponse}"/>.</returns>
    [HttpDelete("/Entry/{EntryID:Guid?}")]
    public async Task<ActionResult<EntryDeleteResponse>> DeleteAsync(
      [FromRoute]EntryDeleteRequest request)
    {
      this.loggerService.LogInformation("DeleteAsync: Validating model state");

      if (!this.ModelState.IsValid)
      {
        this.loggerService.LogInformation("DeleteAsync: Model state not valid");

        if (this.loggerService.IsEnabled(LogLevel.Debug))
        {
          this.loggerService.LogInformation(
            "DeleteAsync: State is {state}",
            JsonSerializer.Serialize(new SerializableError(this.ModelState)));
        }

        this.ModelState.Clear();

        this.ModelState.AddModelError(
          Resources.Strings.Entry,
          Resources.Strings.RequestNotValid);

        return this.BadRequest(this.ModelState);
      }

      this.loggerService.LogInformation("DeleteAsync: Checking authorisation policy");

      var requestAuthorisationResult = await this.authorisationService.AuthorizeAsync(
        this.User, request, EntryPolicies.DeletePolicy);

      if (!requestAuthorisationResult.Succeeded)
      {
        this.loggerService.LogInformation("DeleteAsync: Authorisation failed");

        return this.Forbid();
      }

      this.loggerService.LogInformation("DeleteAsync: Validating request");

      var requestValidator =
        new EntryDeleteRequestValidator(this.storageService).Validate(request);

      if (!requestValidator.IsValid)
      {
        this.loggerService.LogInformation("DeleteAsync: Request not valid");

        return this.BadRequest(requestValidator.AsModelState());
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
      this.loggerService.LogInformation("ListAsync: Validating model state");

      if (!this.ModelState.IsValid)
      {
        this.loggerService.LogInformation("ListAsync: Model state not valid");

        if (this.loggerService.IsEnabled(LogLevel.Debug))
        {
          this.loggerService.LogInformation(
            "ListAsync: State is {state}",
            JsonSerializer.Serialize(new SerializableError(this.ModelState)));
        }

        this.ModelState.Clear();

        this.ModelState.AddModelError(
          Resources.Strings.Entry,
          Resources.Strings.RequestNotValid);

        return this.BadRequest(this.ModelState);
      }

      this.loggerService.LogInformation("ListAsync: Checking authorisation policy");

      var requestAuthorisationResult = await this.authorisationService.AuthorizeAsync(
        this.User, request, EntryPolicies.ListPolicy);

      if (!requestAuthorisationResult.Succeeded)
      {
        this.loggerService.LogInformation("ListAsync: Authorisation failed");

        return this.Forbid();
      }

      this.loggerService.LogInformation("ListAsync: Validating request");

      var requestValidator =
        new EntryListRequestValidator(this.storageService).Validate(request);

      if (!requestValidator.IsValid)
      {
        this.loggerService.LogInformation("ListAsync: Request not valid");

        return this.BadRequest(requestValidator.AsModelState());
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
      this.loggerService.LogInformation("ViewAsync: Validating model state");

      if (!this.ModelState.IsValid)
      {
        this.loggerService.LogInformation("ViewAsync: Model state not valid");

        if (this.loggerService.IsEnabled(LogLevel.Debug))
        {
          this.loggerService.LogInformation(
            "ViewAsync: State is {state}",
            JsonSerializer.Serialize(new SerializableError(this.ModelState)));
        }

        this.ModelState.Clear();

        this.ModelState.AddModelError(
          Resources.Strings.Entry,
          Resources.Strings.RequestNotValid);

        return this.BadRequest(this.ModelState);
      }

      this.loggerService.LogInformation("ViewAsync: Checking authorisation policy");

      var requestAuthorisationResult = await this.authorisationService.AuthorizeAsync(
        this.User, request, EntryPolicies.ViewPolicy);

      if (!requestAuthorisationResult.Succeeded)
      {
        this.loggerService.LogInformation("ViewAsync: Authorisation failed");

        return this.Forbid();
      }

      this.loggerService.LogInformation("ViewAsync: Validating request");

      var requestValidator =
        new EntryViewRequestValidator(this.storageService).Validate(request);

      if (!requestValidator.IsValid)
      {
        this.loggerService.LogInformation("ViewAsync: Request not valid");

        return this.BadRequest(requestValidator.AsModelState());
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
  }
}
