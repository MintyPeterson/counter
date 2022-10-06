// <copyright file="ValidationResultExtensions.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Extensions
{
  using FluentValidation.Results;
  using Microsoft.AspNetCore.Mvc.ModelBinding;

  /// <summary>
  /// Provides extension methods for <see cref="ValidationResult"/>.
  /// </summary>
  public static class ValidationResultExtensions
  {
    /// <summary>
    /// Returns a <see cref="ValidationResult"/> as a <see cref="ModelStateDictionary"/>.
    /// </summary>
    /// <param name="validationResult">The <see cref="ValidationResult"/>.</param>
    /// <returns>A <see cref="ModelStateDictionary"/>.</returns>
    public static ModelStateDictionary AsModelState(this ValidationResult validationResult)
    {
      var modelState = new ModelStateDictionary();

      if (!validationResult.IsValid)
      {
        foreach (var error in validationResult.Errors)
        {
          modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
      }

      return modelState;
    }
  }
}
