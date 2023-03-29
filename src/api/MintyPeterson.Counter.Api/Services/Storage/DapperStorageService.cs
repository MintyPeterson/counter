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
    public override StorageServiceResult<EntryNewResult> EntryNew(EntryNewQuery query)
    {
      var result = new StorageServiceResult<EntryNewResult>();

      try
      {
        using (var connection = new SqlConnection(this.connectionString))
        {
          result.Result = connection.QuerySingleOrDefault<EntryNewResult>(
            Resources.Queries.EntryNewInsert,
            query);
        }
      }
      catch (SqlException error)
      {
        result.Exception = error;
      }

      return result;
    }

    /// <inheritdoc/>
    public override StorageServiceResult<EntryGetResult> EntryGet(EntryGetQuery query)
    {
      var result = new StorageServiceResult<EntryGetResult>();

      try
      {
        using (var connection = new SqlConnection(this.connectionString))
        {
          result.Result = connection.QuerySingleOrDefault<EntryGetResult>(
            Resources.Queries.EntryGetSelect,
            query);
        }
      }
      catch (SqlException error)
      {
        result.Exception = error;
      }

      return result;
    }

    /// <inheritdoc/>
    public override StorageServiceResult<EntryDeleteResult> EntryDelete(EntryDeleteQuery query)
    {
      var result = new StorageServiceResult<EntryDeleteResult>();

      try
      {
        using (var connection = new SqlConnection(this.connectionString))
        {
          result.Result = connection.QuerySingleOrDefault<EntryDeleteResult>(
            Resources.Queries.EntryDeleteUpdate,
            query);
        }
      }
      catch (SqlException error)
      {
        result.Exception = error;
      }

      return result;
    }

    /// <inheritdoc/>
    public override StorageServiceResult<EntryListResult> EntryList(EntryListQuery query)
    {
      var result = new StorageServiceResult<EntryListResult>();

      try
      {
        using (var connection = new SqlConnection(this.connectionString))
        {
          result.Result = new EntryListResult
          {
            Entries = connection.Query<EntryListEntryResult>(
              Resources.Queries.EntryListSelect,
              query),
          };
        }
      }
      catch (SqlException error)
      {
        result.Exception = error;
      }

      return result;
    }

    /// <inheritdoc/>
    public override StorageServiceResult<EntryEditResult> EntryEdit(EntryEditQuery query)
    {
      var result = new StorageServiceResult<EntryEditResult>();

      try
      {
        using (var connection = new SqlConnection(this.connectionString))
        {
          result.Result = connection.QuerySingleOrDefault<EntryEditResult>(
            Resources.Queries.EntryEditUpdate,
            query);
        }
      }
      catch (SqlException error)
      {
        result.Exception = error;
      }

      return result;
    }

    /// <inheritdoc/>
    public override StorageServiceResult<UserSynchroniseResult> UserSynchronise(
      UserSynchroniseQuery query)
    {
      var result = new StorageServiceResult<UserSynchroniseResult>();

      try
      {
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
              result.Result = new UserSynchroniseResult();
            }
            else
            {
              result.Exception = new Exception(
                "No rows affected. The user was not updated or inserted.");
            }
          }
        }
      }
      catch (SqlException error)
      {
        result.Exception = error;
      }

      return result;
    }
  }
}
