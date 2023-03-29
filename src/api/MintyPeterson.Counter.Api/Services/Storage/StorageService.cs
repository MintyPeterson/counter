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
    public abstract StorageServiceResult<EntryNewResult> EntryNew(EntryNewQuery query);

    /// <inheritdoc/>
    public abstract StorageServiceResult<EntryGetResult> EntryGet(EntryGetQuery query);

    /// <inheritdoc/>
    public abstract StorageServiceResult<EntryDeleteResult> EntryDelete(EntryDeleteQuery query);

    /// <inheritdoc/>
    public abstract StorageServiceResult<EntryListResult> EntryList(EntryListQuery query);

    /// <inheritdoc/>
    public abstract StorageServiceResult<EntryEditResult> EntryEdit(EntryEditQuery query);

    /// <inheritdoc/>
    public abstract StorageServiceResult<UserSynchroniseResult> UserSynchronise(
      UserSynchroniseQuery query);
  }
}
