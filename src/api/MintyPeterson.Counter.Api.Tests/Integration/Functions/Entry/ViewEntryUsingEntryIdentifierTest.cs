// <copyright file="ViewEntryUsingEntryIdentifierTest.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Tests.Integration.Functions.Entry
{
  using System.Net;
  using System.Net.Http.Json;
  using FluentAssertions;
  using MintyPeterson.Counter.Api.Models.Responses;
  using Xunit;

  /// <summary>
  /// Tests if the user can view an existing entry by specifying the entry identifier.
  /// </summary>
  public class ViewEntryUsingEntryIdentifierTest : IntegrationTest
  {
    /// <summary>
    /// Stores the <see cref="HttpResponseMessage"/>.
    /// </summary>
    private HttpResponseMessage? response;

    /// <summary>
    /// Stores the <see cref="HttpResponseMessage"/> content.
    /// </summary>
    private EntryViewResponse? responseContent;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewEntryUsingEntryIdentifierTest"/> class.
    /// </summary>
    /// <param name="fixture">A <see cref="CounterWebApplicationFactory"/>.</param>
    public ViewEntryUsingEntryIdentifierTest(CounterWebApplicationFactory fixture)
      : base(fixture)
    {
    }

    /// <summary>
    /// Tests if the status code is OK (200).
    /// </summary>
    [Fact]
    public void StatusCodeShouldBeOk() =>
      this.response!.StatusCode.Should().Be(HttpStatusCode.OK);

    /// <summary>
    /// Tests if the entry identifier is not empty.
    /// </summary>
    [Fact]
    public void EntryIdentiferShouldNotBeEmpty() =>
      this.responseContent!.EntryId.Should().NotBeEmpty();

    /// <summary>
    /// Tests if the entry date has a value.
    /// </summary>
    [Fact]
    public void EntryDateShouldBePopulated() =>
      this.responseContent!.EntryDate.Should().Be(DateTime.Today);

    /// <summary>
    /// Tests if the entry has a value.
    /// </summary>
    [Fact]
    public void EntryShouldBePopulated() =>
      this.responseContent!.Entry.Should().Be(10);

    /// <summary>
    /// Tests if notes has a value.
    /// </summary>
    [Fact]
    public void NotesShouldBePopulated() =>
      this.responseContent!.Notes.Should().Be("Ten");

    /// <summary>
    /// Tests if the estimate indicator has a value.
    /// </summary>
    [Fact]
    public void EstimateIndicatorShouldBePopulated() =>
      this.responseContent!.IsEstimate.Should().Be(true);

    /// <inheritdoc/>
    public override Task InitializeAsync()
    {
      return this.SendRequestAsync();
    }

    /// <summary>
    /// Sends a request to the system.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    private async Task SendRequestAsync()
    {
      await this.ResetAndPopulateDatabaseAsync(
        @"
          DECLARE @CurrentDateTime datetimeoffset = SYSDATETIMEOFFSET()
          DECLARE @CurrentDate date = GETDATE()
          DECLARE @UserID nvarchar(450) = '00000000-0000-0000-0000-000000000001'
          DECLARE @EntryID nvarchar(450) = '00000000-0000-0000-0000-000000000001'

          INSERT INTO Users (
            UserID
           ,CreatedDateTime
           ,UpdatedDateTime
           ,Name
           ,Email
          )
          VALUES (
            @UserID
           ,@CurrentDateTime
           ,@CurrentDateTime
           ,'Integration Tester'
           ,'integration.tester@drtomcook.uk'
          )

          INSERT INTO Entries (
            EntryID
           ,CreatedDateTime
           ,CreatedByUserID
           ,UpdatedDateTime
           ,UpdatedByUserID
           ,EntryDate
           ,Entry
           ,Notes
           ,IsEstimate
          )
          VALUES (
            @EntryID
           ,@CurrentDateTime
           ,@UserID
           ,@CurrentDateTime
           ,@UserID
           ,@CurrentDate
           ,'10'
           ,'Ten'
           ,1
          )
        ");

      this.response =
        await this.Client.GetAsync("/Entry/00000000-0000-0000-0000-000000000001");

      if (this.response.IsSuccessStatusCode)
      {
        this.responseContent =
          await this.response.Content.ReadFromJsonAsync<EntryViewResponse>();
      }
    }
  }
}
