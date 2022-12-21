// <copyright file="EntryProfileTest.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Tests.Unit.Maps
{
  using AutoMapper;
  using MintyPeterson.Counter.Api.Maps;
  using Xunit;

  /// <summary>
  /// Provides tests for <see cref="EntryProfile"/>.
  /// </summary>
  public class EntryProfileTest : UnitTest
  {
    /// <summary>
    /// Tests if the profile throws any configration exception errors.
    /// </summary>
    [Fact]
    public void ShouldThrowIfConfigurationNotValid()
    {
      var configuration = new MapperConfiguration(
        config => config.AddProfile<EntryProfile>());

      configuration.AssertConfigurationIsValid();
    }
  }
}
