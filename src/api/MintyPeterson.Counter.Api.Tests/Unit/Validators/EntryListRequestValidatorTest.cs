// <copyright file="EntryListRequestValidatorTest.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Tests.Unit.Validators
{
  using FluentValidation.TestHelper;
  using MintyPeterson.Counter.Api.Models.Requests;
  using MintyPeterson.Counter.Api.Services.Storage;
  using MintyPeterson.Counter.Api.Tests.Unit;
  using MintyPeterson.Counter.Api.Validators;
  using Moq;
  using Xunit;

  /// <summary>
  /// Provides tests for <see cref="EntryListRequestValidator"/>.
  /// </summary>
  public class EntryListRequestValidatorTest : UnitTest
  {
    /// <summary>
    /// Stores the validator.
    /// </summary>
    private readonly EntryListRequestValidator validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryListRequestValidatorTest"/> class.
    /// </summary>
    public EntryListRequestValidatorTest()
    {
      this.validator = new EntryListRequestValidator(new Mock<IStorageService>().Object);
    }

    /// <summary>
    /// Tests if an error occurrs when minimal data is supplied.
    /// </summary>
    [Fact]
    public void ShouldNotErrorOnMinimalData()
    {
      var result = this.validator.TestValidate(
        new EntryListRequest());

      result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests if an error occurrs when complete data is supplied.
    /// </summary>
    [Fact]
    public void ShouldNotErrorOnCompleteData()
    {
      var result = this.validator.TestValidate(
        new EntryListRequest());

      result.ShouldNotHaveAnyValidationErrors();
    }
  }
}
