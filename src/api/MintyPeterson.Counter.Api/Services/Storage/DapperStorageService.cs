// <copyright file="DapperStorageService.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage
{
  using System.Data.SqlClient;
  using Dapper;
  using MintyPeterson.Counter.Api.Services.Storage.Queries;
  using MintyPeterson.Counter.Api.Services.Storage.Results;

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

    /// <inheritdoc/>
    public override EntryNewResult? EntryNew(EntryNewQuery query)
    {
      using (var connection = new SqlConnection(this.connectionString))
      {
        return connection.QuerySingleOrDefault<EntryNewResult>(
          Resources.Queries.EntryNewInsert,
          query);
      }
    }

    /// <inheritdoc/>
    public override EntryGetResult? EntryGet(EntryGetQuery query)
    {
      using (var connection = new SqlConnection(this.connectionString))
      {
        return connection.QuerySingleOrDefault<EntryGetResult>(
          Resources.Queries.EntryGetSelect,
          query);
      }
    }

    /// <inheritdoc/>
    public override EntryDeleteResult? EntryDelete(EntryDeleteQuery query)
    {
      using (var connection = new SqlConnection(this.connectionString))
      {
        return connection.QuerySingleOrDefault<EntryDeleteResult>(
          Resources.Queries.EntryDeleteUpdate,
          query);
      }
    }

    /// <inheritdoc/>
    public override UserSynchroniseResult? UserSynchronise(UserSynchroniseQuery query)
    {
      UserSynchroniseResult? result = null;

      using (var connection = new SqlConnection(this.connectionString))
      {
        connection.Open();

        using (var transaction = connection.BeginTransaction())
        {
          var numberOfRowsAffected =
            connection.Execute(Resources.Queries.UserSynchroniseUpdate, query, transaction);

          if (numberOfRowsAffected == 0)
          {
            numberOfRowsAffected =
              connection.Execute(Resources.Queries.UserSynchroniseInsert, query, transaction);
          }

          transaction.Commit();

          if (numberOfRowsAffected > 0)
          {
            result = new UserSynchroniseResult();
          }
        }
      }

      return result;
    }
  }
}
