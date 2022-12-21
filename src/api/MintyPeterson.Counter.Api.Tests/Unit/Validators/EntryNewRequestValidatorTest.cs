// <copyright file="EntryNewRequestValidatorTest.cs" company="Tom Cook">
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
  /// Provides tests for <see cref="EntryNewRequestValidator"/>.
  /// </summary>
  public class EntryNewRequestValidatorTest : UnitTest
  {
    /// <summary>
    /// Stores the validator.
    /// </summary>
    private readonly EntryNewRequestValidator validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryNewRequestValidatorTest"/> class.
    /// </summary>
    public EntryNewRequestValidatorTest()
    {
      this.validator = new EntryNewRequestValidator(new Mock<IStorageService>().Object);
    }

    /// <summary>
    /// Tests if an error occurrs when minimal data is supplied.
    /// </summary>
    [Fact]
    public void ShouldNotErrorOnMinimalData()
    {
      var result = this.validator.TestValidate(
        new EntryNewRequest
        {
          EntryDate = DateTime.Today,
          Entry = 10,
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
        new EntryNewRequest
        {
          EntryDate = DateTime.Today,
          Entry = 10,
          Notes = string.Empty,
        });

      result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests if an error occurs when the entry date is missing.
    /// </summary>
    [Fact]
    public void ShouldErrorOnMissingEntryDate()
    {
      var result = this.validator.TestValidate(
        new EntryNewRequest
        {
          EntryDate = null,
        });

      result.ShouldHaveValidationErrorFor(m => m.EntryDate);
    }

    /// <summary>
    /// Tests if an error occurs when the entry is missing.
    /// </summary>
    [Fact]
    public void ShouldErrorOnMissingEntry()
    {
      var result = this.validator.TestValidate(
        new EntryNewRequest
        {
          Entry = null,
        });

      result.ShouldHaveValidationErrorFor(m => m.Entry);
    }

    /// <summary>
    /// Tests if an error occurs when the entry is out of range.
    /// </summary>
    /// <param name="entry">The entry to test.</param>
    [Theory]
    [InlineData(-100000)]
    [InlineData(-10.5)]
    [InlineData(0.5)]
    [InlineData(10.5)]
    [InlineData(100000)]
    public void ShouldErrorOnEntryOutOfRange(decimal entry)
    {
      var result = this.validator.TestValidate(
        new EntryNewRequest
        {
          Entry = entry,
        });

      result.ShouldHaveValidationErrorFor(m => m.Entry);
    }
  }
}
