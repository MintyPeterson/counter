// <copyright file="EntryViewRequestValidator.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Validators
{
  using FluentValidation;
  using MintyPeterson.Counter.Api.Models.Requests;
  using MintyPeterson.Counter.Api.Services.Storage;

  /// <summary>
  /// Provides a <see cref="AbstractValidator{EntryViewRequest}"/>.
  /// </summary>
  public class EntryViewRequestValidator : AbstractValidator<EntryViewRequest>
  {
    /// <summary>
    /// Stores the <see cref="IStorageService"/> dependency.
    /// </summary>
    private readonly IStorageService storageService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryViewRequestValidator"/> class.
    /// </summary>
    /// <param name="storageService">An <see cref="IStorageService"/>.</param>
    public EntryViewRequestValidator(IStorageService storageService)
    {
      this.storageService = storageService;

      this.RuleFor(
        r => r.EntryId)
        .NotEmpty()
        .WithMessage(
          Resources.Strings.EntryIdentifierParameterRequired);
    }
  }
}
