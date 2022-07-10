using eTournamentAPI.Controllers;
using eTournamentAPI.Data.Services;
using eTournamentAPI.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;

namespace eTournamentAPIUnitTest
{
    public class MatchesControllerTest
    {
        [Fact]
        public async Task Get_all_matches_returns_correct_number()
        {
            // Arrange 
            int count = 6;
            var fakeMatches = A.CollectionOfDummy<Match>(count).AsEnumerable();
            var dataStore = A.Fake<IMatchService>();
            A.CallTo(() => dataStore.GetAllAsync(n => n.Team)).Returns(Task.FromResult(fakeMatches));
            var controlelr = new MatchesController(dataStore);

            // Act
            var actionResult = await controlelr.Index();

            // Assert
            var result = actionResult as OkObjectResult;
            var returnsMatches = result.Value as IEnumerable<Match>;
            Assert.Equal(count, returnsMatches.Count());
        }
    }
}