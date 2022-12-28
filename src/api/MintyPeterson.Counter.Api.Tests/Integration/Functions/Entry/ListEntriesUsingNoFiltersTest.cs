// <copyright file="ListEntriesUsingNoFiltersTest.cs" company="Tom Cook">
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
  /// Tests if the user can view a list of entries without specifying any filters.
  /// </summary>
  public class ListEntriesUsingNoFiltersTest : IntegrationTest
  {
    /// <summary>
    /// Stores the <see cref="HttpResponseMessage"/>.
    /// </summary>
    private HttpResponseMessage? response;

    /// <summary>
    /// Stores the <see cref="HttpResponseMessage"/> content.
    /// </summary>
    private EntryListResponse? responseContent;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListEntriesUsingNoFiltersTest"/> class.
    /// </summary>
    /// <param name="fixture">A <see cref="CounterWebApplicationFactory"/>.</param>
    public ListEntriesUsingNoFiltersTest(CounterWebApplicationFactory fixture)
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
    /// Tests for the correct number of groups.
    /// </summary>
    [Fact]
    public void GroupCountShouldBeNumberOfGroups() =>
      this.responseContent!.Groups.Should().HaveCount(2);

    /// <summary>
    /// Tests if a group has a name.
    /// </summary>
    /// <remarks>This does not test a specific name as it will be localised.</remarks>
    [Fact]
    public void GroupNameShouldNotBeEmpty() =>
      this.responseContent!.Groups!.Last().Name.Should().NotBeNullOrEmpty();

    /// <summary>
    /// Tests for the correct number of entries in a group.
    /// </summary>
    [Fact]
    public void GroupEntriesShouldBeNumberOfEntriesInGroup() =>
      this.responseContent!.Groups!.Last().Entries.Should().HaveCount(2);

    /// <summary>
    /// Tests if a group total has been calculated propertly.
    /// </summary>
    [Fact]
    public void GroupTotalShouldBeSumOfEntries() =>
      this.responseContent!.Groups!.Last().Total.Should().Be(30);

    /// <summary>
    /// Tests if an entry has a value.
    /// </summary>
    [Fact]
    public void EntryValueShouldBePopulated() =>
      this.responseContent!.Groups!.First().Entries!.First().Entry.Should().Be(30);

    /// <summary>
    /// Tests if an entry has a note.
    /// </summary>
    [Fact]
    public void EntryNotesShouldBePopulated() =>
      this.responseContent!.Groups!.First().Entries!.First().Notes.Should().Be("Thirty");

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
           ,'identity.tester@drtomcook.uk'
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
          )
          VALUES
            (NEWID(), @CurrentDateTime, @UserID, @CurrentDateTime, @UserID, DATEADD(DAY, -2, @CurrentDate), '10', 'Ten')
           ,(NEWID(), @CurrentDateTime, @UserID, @CurrentDateTime, @UserID, DATEADD(DAY, -2, @CurrentDate), '20', 'Twenty')
           ,(NEWID(), @CurrentDateTime, @UserID, @CurrentDateTime, @UserID, DATEADD(DAY, -1, @CurrentDate), '30', 'Thirty')
        ");

      this.response = await this.Client.GetAsync("/Entries");

      if (this.response.IsSuccessStatusCode)
      {
        this.responseContent =
          await this.response.Content.ReadFromJsonAsync<EntryListResponse>();
      }
    }
  }
}
