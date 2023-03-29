// <copyright file="EntryEditHandlerTest.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Tests.Unit.Policies
{
  using System.Security.Claims;
  using AutoMapper;
  using FluentAssertions;
  using Microsoft.AspNetCore.Authorization;
  using MintyPeterson.Counter.Api.Models.Requests;
  using MintyPeterson.Counter.Api.Policies;
  using MintyPeterson.Counter.Api.Services.Storage;
  using MintyPeterson.Counter.Api.Services.Storage.Queries;
  using MintyPeterson.Counter.Api.Services.Storage.Results;
  using Moq;
  using Xunit;

  /// <summary>
  /// Provides tests for <see cref="EntryEditHandler"/>.
  /// </summary>
  public class EntryEditHandlerTest
  {
    /// <summary>
    /// Stores the default name identifier claim.
    /// </summary>
    private readonly Guid defaultNameIdentifierClaim;

    /// <summary>
    /// Stores the default user.
    /// </summary>
    private readonly ClaimsPrincipal defaultUser;

    /// <summary>
    /// Stores the default resource.
    /// </summary>
    private readonly EntryEditRequest defaultResource;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryEditHandlerTest"/> class.
    /// </summary>
    public EntryEditHandlerTest()
    {
      this.defaultNameIdentifierClaim = Guid.NewGuid();

      this.defaultUser = new ClaimsPrincipal(new ClaimsIdentity(
        new Claim[]
        {
          new Claim(ClaimTypes.NameIdentifier, this.defaultNameIdentifierClaim.ToString()),
        }));

      this.defaultResource = new EntryEditRequest { EntryId = Guid.NewGuid() };
    }

    /// <summary>
    /// Tests if the handler succeeds when the entry does not exist.
    /// </summary>
    [Fact]
    public async void ShouldSucceedWhenEntryDoesNotExist()
    {
      var context = new AuthorizationHandlerContext(
        new List<IAuthorizationRequirement> { new EntryEditRequirements() },
        this.defaultUser,
        this.defaultResource);

      var mapperService = new Mock<IMapper>();
      var storageService = new Mock<IStorageService>();

      storageService.Setup(
        options => options.EntryGet(It.IsAny<EntryGetQuery>()))
          .Returns(new StorageServiceResult<EntryGetResult>());

      var handler = new EntryEditHandler(storageService.Object, mapperService.Object);

      await handler.HandleAsync(context);

      context.HasSucceeded.Should().BeTrue();
    }

    /// <summary>
    /// Tests if the handler succeeds when the entry was created by the user.
    /// </summary>
    [Fact]
    public async void ShouldSucceedWhenUserCreatedEntry()
    {
      var context = new AuthorizationHandlerContext(
        new List<IAuthorizationRequirement> { new EntryEditRequirements() },
        this.defaultUser,
        this.defaultResource);

      var mapperService = new Mock<IMapper>();
      var storageService = new Mock<IStorageService>();

      storageService.Setup(
        options => options.EntryGet(It.IsAny<EntryGetQuery>()))
          .Returns(
            new StorageServiceResult<EntryGetResult>
            {
              Result = new EntryGetResult
              {
                CreatedByUserId = this.defaultNameIdentifierClaim.ToString(),
              },
            });

      var handler = new EntryEditHandler(storageService.Object, mapperService.Object);

      await handler.HandleAsync(context);

      context.HasSucceeded.Should().BeTrue();
    }

    /// <summary>
    /// Tests if the handler fails when the entry was not created by the user.
    /// </summary>
    [Fact]
    public async void ShouldFailWhenUserDidNotCreateEntry()
    {
      var context = new AuthorizationHandlerContext(
        new List<IAuthorizationRequirement> { new EntryEditRequirements() },
        this.defaultUser,
        this.defaultResource);

      var mapperService = new Mock<IMapper>();
      var storageService = new Mock<IStorageService>();

      storageService.Setup(
        options => options.EntryGet(It.IsAny<EntryGetQuery>()))
          .Returns(
            new StorageServiceResult<EntryGetResult>
            {
              Result = new EntryGetResult
              {
                CreatedByUserId = Guid.NewGuid().ToString(),
              },
            });

      var handler = new EntryEditHandler(storageService.Object, mapperService.Object);

      await handler.HandleAsync(context);

      context.HasSucceeded.Should().BeFalse();
    }
  }
}
