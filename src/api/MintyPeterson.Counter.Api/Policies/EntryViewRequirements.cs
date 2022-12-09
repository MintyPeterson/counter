// <copyright file="EntryViewRequirements.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Policies
{
  using Microsoft.AspNetCore.Authorization;

  /// <summary>
  /// Specifies the requirements for the entry view <see cref="IAuthorizationHandler"/>.
  /// </summary>
  public class EntryViewRequirements : IAuthorizationRequirement
  {
  }
}
