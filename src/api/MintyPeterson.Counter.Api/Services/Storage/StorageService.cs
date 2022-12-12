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
    public abstract EntryGetResult? EntryGet(EntryGetQuery query);

    /// <inheritdoc/>
    public abstract EntryDeleteResult? EntryDelete(EntryDeleteQuery query);

    /// <inheritdoc/>
    public abstract EntryListResult? EntryList(EntryListQuery query);

    /// <inheritdoc/>
    public abstract EntryEditResult? EntryEdit(EntryEditQuery query);

    /// <inheritdoc/>
    public abstract UserSynchroniseResult? UserSynchronise(UserSynchroniseQuery query);
  }
}
