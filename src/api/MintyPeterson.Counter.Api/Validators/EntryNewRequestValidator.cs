// <copyright file="EntryNewRequestValidator.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Validators
{
  using FluentValidation;
  using MintyPeterson.Counter.Api.Models.Requests;
  using MintyPeterson.Counter.Api.Services.Storage;

  /// <summary>
  /// Provides a <see cref="AbstractValidator{EntryNewRequest}"/>.
  /// </summary>
  public class EntryNewRequestValidator : AbstractValidator<EntryNewRequest>
  {
    /// <summary>
    /// Stores the <see cref="IStorageService"/> dependency.
    /// </summary>
    private readonly IStorageService storageService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryNewRequestValidator"/> class.
    /// </summary>
    /// <param name="storageService">An <see cref="IStorageService"/>.</param>
    public EntryNewRequestValidator(IStorageService storageService)
    {
      this.storageService = storageService;

      this.RuleFor(
        r => r.EntryDate)
        .NotEmpty()
        .WithMessage(
          Resources.Strings.EntryDateParameterRequired);

      this.RuleFor(
        r => r.Entry)
        .Cascade(
          CascadeMode.Stop)
        .NotNull()
        .WithMessage(
          Resources.Strings.EntryParameterRequired)
        .ScalePrecision(
          0, 5)
        .WithMessage(
          Resources.Strings.EntryParameterOutOfRange);
    }
  }
}
