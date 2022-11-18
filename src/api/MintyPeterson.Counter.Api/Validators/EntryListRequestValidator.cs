// <copyright file="EntryListRequestValidator.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Validators
{
  using FluentValidation;
  using MintyPeterson.Counter.Api.Models.Requests;
  using MintyPeterson.Counter.Api.Services.Storage;

  /// <summary>
  /// Provides a <see cref="AbstractValidator{EntryListRequest}"/>.
  /// </summary>
  public class EntryListRequestValidator : AbstractValidator<EntryListRequest>
  {
    /// <summary>
    /// Stores the <see cref="IStorageService"/> dependency.
    /// </summary>
    private readonly IStorageService storageService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryListRequestValidator"/> class.
    /// </summary>
    /// <param name="storageService">An <see cref="IStorageService"/>.</param>
    public EntryListRequestValidator(IStorageService storageService)
    {
      this.storageService = storageService;
    }
  }
}
