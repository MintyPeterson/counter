// <copyright file="EntryViewHandlerTest.cs" company="Tom Cook">
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
  /// Provides tests for <see cref="EntryViewHandler"/>.
  /// </summary>
  public class EntryViewHandlerTest
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
    private readonly EntryViewRequest defaultResource;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryViewHandlerTest"/> class.
    /// </summary>
    public EntryViewHandlerTest()
    {
      this.defaultNameIdentifierClaim = Guid.NewGuid();

      this.defaultUser = new ClaimsPrincipal(new ClaimsIdentity(
        new Claim[]
        {
          new Claim(ClaimTypes.NameIdentifier, this.defaultNameIdentifierClaim.ToString()),
        }));

      this.defaultResource = new EntryViewRequest { EntryId = Guid.NewGuid() };
    }

    /// <summary>
    /// Tests if the handler succeeds when the entry does not exist.
    /// </summary>
    [Fact]
    public async void ShouldSucceedWhenEntryDoesNotExist()
    {
      var context = new AuthorizationHandlerContext(
        new List<IAuthorizationRequirement> { new EntryViewRequirements() },
        this.defaultUser,
        this.defaultResource);

      var mapperService = new Mock<IMapper>();
      var storageService = new Mock<IStorageService>();

      storageService.Setup(
        options => options.EntryGet(It.IsAny<EntryGetQuery>()))
          .Returns(new StorageServiceResult<EntryGetResult>());

      var handler = new EntryViewHandler(storageService.Object, mapperService.Object);

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
        new List<IAuthorizationRequirement> { new EntryViewRequirements() },
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

      var handler = new EntryViewHandler(storageService.Object, mapperService.Object);

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
        new List<IAuthorizationRequirement> { new EntryViewRequirements() },
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

      var handler = new EntryViewHandler(storageService.Object, mapperService.Object);

      await handler.HandleAsync(context);

      context.HasSucceeded.Should().BeFalse();
    }
  }
}
