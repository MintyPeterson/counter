// <copyright file="EntryEditRequestValidatorTest.cs" company="Tom Cook">
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
  /// Provides tests for <see cref="EntryEditRequestValidator"/>.
  /// </summary>
  public class EntryEditRequestValidatorTest : UnitTest
  {
    /// <summary>
    /// Stores the validator.
    /// </summary>
    private readonly EntryEditRequestValidator validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryEditRequestValidatorTest"/> class.
    /// </summary>
    public EntryEditRequestValidatorTest()
    {
      this.validator = new EntryEditRequestValidator(new Mock<IStorageService>().Object);
    }

    /// <summary>
    /// Tests if an error occurrs when minimal data is supplied.
    /// </summary>
    [Fact]
    public void ShouldNotErrorOnMinimalData()
    {
      var result = this.validator.TestValidate(
        new EntryEditRequest
        {
          EntryId = Guid.NewGuid(),
          Body =
            new EntryEditRequestBody
            {
              EntryDate = DateTime.Today,
              Entry = 10,
            },
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
        new EntryEditRequest
        {
          EntryId = Guid.NewGuid(),
          Body =
            new EntryEditRequestBody
            {
              EntryDate = DateTime.Today,
              Entry = 10,
              Notes = string.Empty,
            },
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
        new EntryEditRequest
        {
          EntryId = null,
        });

      result.ShouldHaveValidationErrorFor(m => m.EntryId);
    }

    /// <summary>
    /// Tests if an error occurs when the entry date is missing.
    /// </summary>
    [Fact]
    public void ShouldErrorOnMissingEntryDate()
    {
      var result = this.validator.TestValidate(
        new EntryEditRequest
        {
          Body =
            new EntryEditRequestBody
            {
              EntryDate = null,
            },
        });

      result.ShouldHaveValidationErrorFor("EntryDate");
    }

    /// <summary>
    /// Tests if an error occurs when the entry is missing.
    /// </summary>
    [Fact]
    public void ShouldErrorOnMissingEntry()
    {
      var result = this.validator.TestValidate(
        new EntryEditRequest
        {
          Body =
            new EntryEditRequestBody
            {
              Entry = null,
            },
        });

      result.ShouldHaveValidationErrorFor("Entry");
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
        new EntryEditRequest
        {
          Body =
            new EntryEditRequestBody
            {
              Entry = entry,
            },
        });

      result.ShouldHaveValidationErrorFor("Entry");
    }
  }
}
