// <copyright file="EntryDeleteRequestValidator.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Validators
{
  using FluentValidation;
  using MintyPeterson.Counter.Api.Models.Requests;
  using MintyPeterson.Counter.Api.Services.Storage;

  /// <summary>
  /// Provides a <see cref="AbstractValidator{EntryDeleteRequest}"/>.
  /// </summary>
  public class EntryDeleteRequestValidator : AbstractValidator<EntryDeleteRequest>
  {
    /// <summary>
    /// Stores the <see cref="IStorageService"/> dependency.
    /// </summary>
    private readonly IStorageService storageService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryDeleteRequestValidator"/> class.
    /// </summary>
    /// <param name="storageService">An <see cref="IStorageService"/>.</param>
    public EntryDeleteRequestValidator(IStorageService storageService)
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
