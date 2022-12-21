// <copyright file="IntegrationTest.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Tests.Integration
{
  using System.Data.SqlClient;
  using Microsoft.Extensions.Configuration;
  using Respawn;
  using Xunit;

  /// <summary>
  /// Provides a base class for integration tests against <see cref="CounterWebApplicationFactory"/>.
  /// </summary>
  [Collection("Integration")]
  [Trait("Category", "Integration")]
  public abstract class IntegrationTest : IClassFixture<CounterWebApplicationFactory>
  {
    /// <summary>
    /// Stores the database connection string.
    /// </summary>
    private readonly string connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationTest"/> class.
    /// </summary>
    /// <param name="fixture">A <see cref="CounterWebApplicationFactory"/>.</param>
    public IntegrationTest(CounterWebApplicationFactory fixture)
    {
      this.Factory = fixture;
      this.Client = this.Factory.CreateClient();

      this.connectionString =
        this.Factory.Configuration.GetConnectionString("SqlServer");
    }

    /// <summary>
    /// Gets the <see cref="CounterWebApplicationFactory"/>.
    /// </summary>
    protected CounterWebApplicationFactory Factory { get; private set; }

    /// <summary>
    /// Gets the <see cref="HttpClient"/>.
    /// </summary>
    protected HttpClient Client { get; private set; }

    /// <summary>
    /// Resets and populates the database with test data.
    /// </summary>
    /// <param name="sql">The SQL used to populate the database.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public async Task ResetAndPopulateDatabaseAsync(string sql)
    {
      await this.ResetDatabaseAsync();
      await this.PopulateDatabaseAsync(sql);
    }

    /// <summary>
    /// Resets the database.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public async Task ResetDatabaseAsync()
    {
      var respawner = await Respawner.CreateAsync(this.connectionString, new RespawnerOptions
      {
        WithReseed = true,
      });

      await respawner.ResetAsync(this.connectionString);
    }

    /// <summary>
    /// Populates the database with test data.
    /// </summary>
    /// <param name="sql">The SQL used to populate the database.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public async Task PopulateDatabaseAsync(string sql)
    {
      using (var connection = new SqlConnection(this.connectionString))
      {
        await connection.OpenAsync();

        using (var command = new SqlCommand(sql, connection))
        {
          await command.ExecuteNonQueryAsync();
        }
      }
    }
  }
}
