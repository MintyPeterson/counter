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
    /// Gets an entry.
    /// </summary>
    /// <param name="query">A <see cref="EntryGetQuery"/>.</param>
    /// <returns>A <see cref="EntryGetResult"/>.</returns>
    EntryGetResult? EntryGet(EntryGetQuery query);

    /// <summary>
    /// Deletes an entry.
    /// </summary>
    /// <param name="query">A <see cref="EntryDeleteQuery"/>.</param>
    /// <returns>A <see cref="EntryDeleteResult"/>.</returns>
    EntryDeleteResult? EntryDelete(EntryDeleteQuery query);

    /// <summary>
    /// Lists entries.
    /// </summary>
    /// <param name="query">A <see cref="EntryListQuery"/>.</param>
    /// <returns>A <see cref="EntryListResult"/>.</returns>
    EntryListResult? EntryList(EntryListQuery query);

    /// <summary>
    /// Updates an entry.
    /// </summary>
    /// <param name="query">A <see cref="EntryEditQuery"/>.</param>
    /// <returns>A <see cref="EntryEditResult"/>.</returns>
    EntryEditResult? EntryEdit(EntryEditQuery query);

    /// <summary>
    /// Updates or inserts a user.
    /// </summary>
    /// <param name="query">A <see cref="UserSynchroniseQuery"/>.</param>
    /// <returns>A <see cref="UserSynchroniseResult"/>.</returns>
    UserSynchroniseResult? UserSynchronise(UserSynchroniseQuery query);
  }
}
