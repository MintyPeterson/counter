// <copyright file="IntegrationTest.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Tests.Integration
{
  using System.Data.SqlClient;
  using System.Globalization;
  using System.Text;
  using System.Text.Json;
  using System.Web;
  using Microsoft.Extensions.Configuration;
  using Respawn;
  using Xunit;

  /// <summary>
  /// Provides a base class for integration tests against <see cref="CounterWebApplicationFactory"/>.
  /// </summary>
  [Collection("Integration")]
  [Trait("Category", "Integration")]
  public abstract class IntegrationTest : IClassFixture<CounterWebApplicationFactory>, IAsyncLifetime
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

    /// <inheritdoc/>
    public abstract Task InitializeAsync();

    /// <inheritdoc/>
    public Task DisposeAsync()
    {
      return Task.CompletedTask;
    }

    /// <summary>
    /// Builds <see cref="HttpContent"/> from an object.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="obj">The object.</param>
    /// <returns>The <see cref="HttpContent"/>.</returns>
    protected static HttpContent BuildRequestContent<T>(T obj)
    {
      return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    }

    /// <summary>
    /// Builds a request URI from an object.
    /// </summary>
    /// <param name="endpoint">The HTTP endpoint.</param>
    /// <param name="obj">The object.</param>
    /// <param name="useQueryString">If true, properties are added to the query string.</param>
    /// <returns>A request URI.</returns>
    protected static string BuildRequestUri(
      string endpoint, object? obj = null, bool useQueryString = true)
    {
      if (obj == null)
      {
        return endpoint;
      }

      var query = HttpUtility.ParseQueryString(string.Empty);

      foreach (var property in obj.GetType().GetProperties())
      {
        var value = property.GetValue(obj, null);

        if (value == null)
        {
          continue;
        }

        var segment = string.Concat("{", property.Name, "}");

        // There are some data types that need to be parsed in a
        // specific format: DateTime.
        string? segmentValue;

        if (value is DateTime)
        {
          segmentValue = (value as DateTime?)?.ToString(
            "yyyy-MM-dd'T'HH:mm:ss.fffK",
            CultureInfo.InvariantCulture);
        }
        else
        {
          segmentValue = value.ToString();
        }

        if (endpoint.Contains(segment))
        {
          endpoint = endpoint.Replace(segment, segmentValue);
        }
        else
        {
          if (useQueryString)
          {
            query.Add(property.Name, segmentValue);
          }
        }
      }

      if (query.Count > 0)
      {
        return string.Concat(endpoint, "?", query.ToString());
      }

      return endpoint;
    }
  }
}
