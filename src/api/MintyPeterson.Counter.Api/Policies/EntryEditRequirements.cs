// <copyright file="EntryEditRequirements.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Policies
{
  using Microsoft.AspNetCore.Authorization;

  /// <summary>
  /// Specifies the requirements for the entry new <see cref="IAuthorizationHandler"/>.
  /// </summary>
  public class EntryEditRequirements : IAuthorizationRequirement
  {
  }
}
