// <copyright file="EntryEditRequestValidator.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Validators
{
  using FluentValidation;
  using MintyPeterson.Counter.Api.Models.Requests;
  using MintyPeterson.Counter.Api.Services.Storage;

  /// <summary>
  /// Provides a <see cref="AbstractValidator{EntryEditRequest}"/>.
  /// </summary>
  public class EntryEditRequestValidator : AbstractValidator<EntryEditRequest>
  {
    /// <summary>
    /// Stores the <see cref="IStorageService"/> dependency.
    /// </summary>
    private readonly IStorageService storageService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryEditRequestValidator"/> class.
    /// </summary>
    /// <param name="storageService">An <see cref="IStorageService"/>.</param>
    public EntryEditRequestValidator(IStorageService storageService)
    {
      this.storageService = storageService;

      this.RuleFor(
        r => r.EntryId)
        .NotEmpty()
        .WithMessage(
          Resources.Strings.EntryIdentifierParameterRequired);

      this.RuleFor(
        r => r.Body.EntryDate)
        .NotEmpty()
        .WithMessage(
          Resources.Strings.EntryDateParameterRequired)
      .OverridePropertyName(
        Resources.Strings.EntryDate);

      this.RuleFor(
        r => r.Body.Entry)
        .Cascade(
          CascadeMode.Stop)
        .NotNull()
        .WithMessage(
          Resources.Strings.EntryParameterRequired)
        .PrecisionScale(
          5, 0, false)
        .WithMessage(
          Resources.Strings.EntryParameterOutOfRange)
        .OverridePropertyName(
          Resources.Strings.Entry);
    }
  }
}
