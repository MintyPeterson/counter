// <copyright file="IStorageService.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage
{
  using MintyPeterson.Counter.Api.Services.Storage.Queries;
  using MintyPeterson.Counter.Api.Services.Storage.Results;

  /// <summary>
  /// Provides a mechanism for storing data.
  /// </summary>
  public interface IStorageService
  {
    /// <summary>
    /// Creates a new entry.
    /// </summary>
    /// <param name="query">A <see cref="EntryNewQuery"/>.</param>
    /// <returns>A <see cref="EntryNewResult"/>.</returns>
    EntryNewResult? EntryNew(EntryNewQuery query);

    /// <summary>
    /// Updates or inserts a user.
    /// </summary>
    /// <param name="query">A <see cref="UserSynchroniseQuery"/>.</param>
    /// <returns>A <see cref="UserSynchroniseResult"/>.</returns>
    UserSynchroniseResult? UserSynchronise(UserSynchroniseQuery query);
  }
}
