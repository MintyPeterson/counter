// <copyright file="DapperStorageService.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage
{
  /// <summary>
  /// Provides a <see cref="StorageService"/> using <see cref="Dapper"/>.
  /// </summary>
  public class DapperStorageService : StorageService
  {
    /// <summary>
    /// Stores the connection string.
    /// </summary>
    private readonly string connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="DapperStorageService"/> class.
    /// </summary>
    /// <param name="connectionString">A connection string.</param>
    public DapperStorageService(string connectionString)
    {
      this.connectionString = connectionString;
    }
  }
}
