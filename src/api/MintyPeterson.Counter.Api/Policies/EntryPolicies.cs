﻿// <copyright file="EntryPolicies.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Policies
{
  using Microsoft.AspNetCore.Authorization;

  /// <summary>
  /// Specifies entry authorization policies.
  /// </summary>
  public class EntryPolicies
  {
    /// <summary>
    /// Stores the new policy name.
    /// </summary>
    public const string NewPolicy = "New Entry Policy";

    /// <summary>
    /// Stores the delete policy name.
    /// </summary>
    public const string DeletePolicy = "Delete Entry Policy";

    /// <summary>
    /// Stores the list policy name.
    /// </summary>
    public const string ListPolicy = "List Entry Policy";

    /// <summary>
    /// Adds policies.
    /// </summary>
    /// <param name="options">The <see cref="AuthorizationOptions"/>.</param>
    public static void AddPolicies(AuthorizationOptions options)
    {
      options.AddPolicy(
        EntryPolicies.NewPolicy,
        policy => policy.AddRequirements(new EntryNewRequirements()));

      options.AddPolicy(
        EntryPolicies.DeletePolicy,
        policy => policy.AddRequirements(new EntryDeleteRequirements()));

      options.AddPolicy(
        EntryPolicies.ListPolicy,
        policy => policy.AddRequirements(new EntryListRequirements()));
    }
  }
}
