// <copyright file="StorageService.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage
{
  using MintyPeterson.Counter.Api.Services.Storage.Queries;
  using MintyPeterson.Counter.Api.Services.Storage.Results;

  /// <summary>
  /// Provides an abstract implementation of <see cref="IStorageService"/>.
  /// </summary>
  public abstract class StorageService : IStorageService
  {
    /// <inheritdoc/>
    public abstract EntryNewResult? EntryNew(EntryNewQuery query);

    /// <inheritdoc/>
    public abstract UserSynchroniseResult? UserSynchronise(UserSynchroniseQuery query);
  }
}
