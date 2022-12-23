// <copyright file="EntryNewHandlerTest.cs" company="Tom Cook">
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
  using Moq;
  using Xunit;

  /// <summary>
  /// Provides tests for <see cref="EntryNewHandler"/>.
  /// </summary>
  public class EntryNewHandlerTest
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
    private readonly EntryNewRequest defaultResource;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryNewHandlerTest"/> class.
    /// </summary>
    public EntryNewHandlerTest()
    {
      this.defaultNameIdentifierClaim = Guid.NewGuid();

      this.defaultUser = new ClaimsPrincipal(new ClaimsIdentity(
        new Claim[]
        {
          new Claim(ClaimTypes.NameIdentifier, this.defaultNameIdentifierClaim.ToString()),
        }));

      this.defaultResource = new EntryNewRequest();
    }

    /// <summary>
    /// Tests if the handler succeeds.
    /// </summary>
    [Fact]
    public async void ShouldAlwaysSucceed()
    {
      var context = new AuthorizationHandlerContext(
        new List<IAuthorizationRequirement> { new EntryNewRequirements() },
        this.defaultUser,
        this.defaultResource);

      var mapperService = new Mock<IMapper>();
      var storageService = new Mock<IStorageService>();

      var handler = new EntryNewHandler(storageService.Object, mapperService.Object);

      await handler.HandleAsync(context);

      context.HasSucceeded.Should().BeTrue();
    }
  }
}
