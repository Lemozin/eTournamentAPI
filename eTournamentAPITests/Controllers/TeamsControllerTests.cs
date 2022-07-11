using eTournamentAPI.Data.Services;
using eTournamentAPI.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

namespace eTournamentAPI.Controllers.Tests;

[TestClass]
public class TeamsControllerTests
{
    public void TeamsControllerTest()
    {
        Assert.Fail();
    }

    [TestMethod]
    public async Task IndexTest()
    {
        // Arrange 
        var fakeRecipes = A.CollectionOfDummy<Team>(GetAllTeams().Count).AsEnumerable();
        var dataStore = A.Fake<ITeamService>();
        A.CallTo(() => dataStore.GetAllAsync()).Returns(Task.FromResult(fakeRecipes));
        var controller = new TeamsController(dataStore);

        // Act
        var actionResult = await controller.Index();

        //Assert
        var result = actionResult as OkObjectResult;
        var returnRecipes = result.Value as IEnumerable<Team>;
        Assert.Equals(GetAllTeams().Count, returnRecipes.Count());
    }

    public void CreateTest()
    {
        Assert.Fail();
    }

    public void DetailsTest()
    {
        Assert.Fail();
    }

    public void EditTest()
    {
        Assert.Fail();
    }

    public void DeleteConfirmTest()
    {
        Assert.Fail();
    }

    private List<Team> GetAllTeams()
    {
        var allTeams = new List<Team>();
        allTeams.AddRange(new List<Team>
        {
            new()
            {
                Name = "Team 1",
                Logo = "https://i.postimg.cc/BQnT6KNY/Team-1.jpg",
                Description = "This is the description of the first team"
            },
            new()
            {
                Name = "Team 2",
                Logo = "https://i.postimg.cc/VLK3rXwd/Team-2.jpg",
                Description = "This is the description of the second team"
            },
            new()
            {
                Name = "Team 3",
                Logo = "https://i.postimg.cc/Y9NPpW82/Team-3.jpg",
                Description = "This is the description of the third team"
            },
            new()
            {
                Name = "Team 4",
                Logo = "https://i.postimg.cc/GmhyL8JL/Team-4.jpg",
                Description = "This is the description of the fourth team"
            },
            new()
            {
                Name = "Team 5",
                Logo = "https://i.postimg.cc/XqSsmc57/Team-5.jpg",
                Description = "This is the description of the fifth team"
            }
        });

        return allTeams;
    }
}