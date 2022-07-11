using eTournamentAPI.Data.Enums;
using eTournamentAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eTournamentAPI.Controllers.Tests;

[TestClass]
public class MatchesControllerTests
{
    [TestMethod]
    public void MatchesControllerTest()
    {
        Assert.Fail();
    }

    [TestMethod]
    public void IndexTest()
    {
        Assert.Fail();
    }

    [TestMethod]
    public void FilterTest()
    {
        Assert.Fail();
    }

    [TestMethod]
    public void DetailsTest()
    {
        Assert.Fail();
    }

    [TestMethod]
    public void CreateTest()
    {
        Assert.Fail();
    }

    [TestMethod]
    public void EditTest()
    {
        Assert.Fail();
    }

    public List<Match> GetAllMatches()
    {
        var allMatches = new List<Match>();
        allMatches.AddRange(new List<Match>
        {
            new()
            {
                Name = "Champions League Final",
                Description = "This is the Champions League Final description",
                Price = 39.50,
                ImageURL = "https://i.postimg.cc/pdbT2jX7/Match-1.jpg",
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(10),
                TeamId = 3,
                CoachId = 3,
                MatchCategory = MatchCategory.Final
            },
            new()
            {
                Name = "Copa America Final",
                Description = "This is the Copa America Final description",
                Price = 29.50,
                ImageURL = "https://i.postimg.cc/d31QWm47/Match-2.jpg",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3),
                TeamId = 1,
                CoachId = 1,
                MatchCategory = MatchCategory.Final
            },
            new()
            {
                Name = "Premier League",
                Description = "This is the Premier League description",
                Price = 39.50,
                ImageURL = "https://i.postimg.cc/7L3LbL5L/Match-3.jpg",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                TeamId = 4,
                CoachId = 4,
                MatchCategory = MatchCategory.Quarterfinals
            },
            new()
            {
                Name = "Europa League Final",
                Description = "This is the Europa League Final description",
                Price = 39.50,
                ImageURL = "https://i.postimg.cc/W3s1ZPZw/Match-4.jpg",
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(-5),
                TeamId = 1,
                CoachId = 2,
                MatchCategory = MatchCategory.Semifinals
            },
            new()
            {
                Name = "Euro 2022 Group Stage",
                Description = "This is the Euro 2022 Group Stage description",
                Price = 39.50,
                ImageURL = "https://i.postimg.cc/rmgFCYCX/Match-5.jpg",
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(-2),
                TeamId = 1,
                CoachId = 3,
                MatchCategory = MatchCategory.SecondRound
            },
            new()
            {
                Name = "Premier League - Fourth Round",
                Description = "This is the Premier League - Fourth Round description",
                Price = 39.50,
                ImageURL = "https://i.postimg.cc/0y6fTTbn/Match-6.jpg",
                StartDate = DateTime.Now.AddDays(3),
                EndDate = DateTime.Now.AddDays(20),
                TeamId = 1,
                CoachId = 5,
                MatchCategory = MatchCategory.FourthRound
            }
        });

        return allMatches;
    }
}