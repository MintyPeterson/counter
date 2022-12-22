// <copyright file="EntryDeleteRequestValidatorTest.cs" company="Tom Cook">
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
  /// Provides tests for <see cref="EntryDeleteRequestValidator"/>.
  /// </summary>
  public class EntryDeleteRequestValidatorTest : UnitTest
  {
    /// <summary>
    /// Stores the validator.
    /// </summary>
    private readonly EntryDeleteRequestValidator validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryDeleteRequestValidatorTest"/> class.
    /// </summary>
    public EntryDeleteRequestValidatorTest()
    {
      this.validator = new EntryDeleteRequestValidator(new Mock<IStorageService>().Object);
    }

    /// <summary>
    /// Tests if an error occurrs when minimal data is supplied.
    /// </summary>
    [Fact]
    public void ShouldNotErrorOnMinimalData()
    {
      var result = this.validator.TestValidate(
        new EntryDeleteRequest
        {
          EntryId = Guid.NewGuid(),
        });

      result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests if an error occurrs when complete data is supplied.
    /// </summary>
    [Fact]
    public void ShouldNotErrorOnCompleteData()
    {
      var result = this.validator.TestValidate(
        new EntryDeleteRequest
        {
          EntryId = Guid.NewGuid(),
        });

      result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests if an error occurs when the entry identifier is missing.
    /// </summary>
    [Fact]
    public void ShouldErrorOnMissingEntryIdentifier()
    {
      var result = this.validator.TestValidate(
        new EntryDeleteRequest
        {
          EntryId = null,
        });

      result.ShouldHaveValidationErrorFor(m => m.EntryId);
    }
  }
}
