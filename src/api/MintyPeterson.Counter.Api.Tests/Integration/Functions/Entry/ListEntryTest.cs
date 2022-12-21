// <copyright file="ListEntryTest.cs" company="Tom Cook">
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
  /// Tests if the system can return a list of entries or not.
  /// </summary>
  /// <remarks>This test case should test the default options.</remarks>
  public class ListEntryTest : IntegrationTest, IAsyncLifetime
  {
    /// <summary>
    /// Stores the <see cref="HttpResponseMessage"/>.
    /// </summary>
    private readonly HttpResponseMessage response;

    /// <summary>
    /// Stores the <see cref="HttpResponseMessage"/> content.
    /// </summary>
    private readonly EntryListResponse? responseContent;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListEntryTest"/> class.
    /// </summary>
    /// <param name="fixture">A <see cref="CounterWebApplicationFactory"/>.</param>
    public ListEntryTest(CounterWebApplicationFactory fixture)
      : base(fixture)
    {
      this.response = this.Client.GetAsync("/Entries").Result;

      if (this.response.IsSuccessStatusCode)
      {
        this.responseContent =
          this.response.Content.ReadFromJsonAsync<EntryListResponse>().Result;
      }
    }

    /// <inheritdoc/>
    public Task InitializeAsync()
    {
      return this.ResetAndPopulateDatabaseAsync(
        @"
          DECLARE @CurrentDateTime date = SYSDATETIMEOFFSET()
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
    }

    /// <inheritdoc/>
    public Task DisposeAsync()
    {
      return Task.CompletedTask;
    }

    /// <summary>
    /// Tests if the status code is OK (200).
    /// </summary>
    [Fact]
    public void StatusCodeShouldBeOk() =>
      this.response.StatusCode.Should().Be(HttpStatusCode.OK);

    /// <summary>
    /// Tests if the groups property has the correct count.
    /// </summary>
    [Fact]
    public void GroupsShouldHaveCorrectCount() =>
      this.responseContent!.Groups.Should().HaveCount(2);

    /// <summary>
    /// Tests if a group has a name.
    /// </summary>
    /// <remarks>This does not test a specific name as it will be localised.</remarks>
    [Fact]
    public void GroupShouldHaveCorrectName() =>
      this.responseContent!.Groups!.Last().Name.Should().NotBeNullOrEmpty();

    /// <summary>
    /// Tests if a group has the correct entry count.
    /// </summary>
    [Fact]
    public void GroupShouldHaveCorrectEntryCount() =>
      this.responseContent!.Groups!.Last().Entries.Should().HaveCount(2);

    /// <summary>
    /// Tests if a group has the correct total.
    /// </summary>
    [Fact]
    public void GroupShouldHaveCorrectTotal() =>
      this.responseContent!.Groups!.Last().Total.Should().Be(30);

    /// <summary>
    /// Tests if an entry has the correct value.
    /// </summary>
    [Fact]
    public void EntryShouldHaveCorrectValue() =>
      this.responseContent!.Groups!.First().Entries!.First().Entry.Should().Be(30);

    /// <summary>
    /// Tests if an entry has the correct note.
    /// </summary>
    [Fact]
    public void EntryShouldHaveCorrectNote() =>
      this.responseContent!.Groups!.First().Entries!.First().Notes.Should().Be("Thirty");
  }
}
