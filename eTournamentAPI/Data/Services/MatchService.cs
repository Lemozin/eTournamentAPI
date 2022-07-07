using System.Linq;
using System.Threading.Tasks;
using eTournamentAPI.Data.Base;
using eTournamentAPI.Data.ViewModels;
using eTournamentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace eTournamentAPI.Data.Services
{
    public class MatchService : EntityBaseRepository<Match>, IMatchService
    {
        private readonly AppDbContext _context;

        public MatchService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewMatchAsync(NewMatchVM data)
        {
            var newMovie = new Match
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                ImageURL = data.ImageURL,
                TeamId = data.TeamId,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                MatchCategory = data.MatchCategory,
                CoachId = data.CoachId
            };
            await _context.Matches.AddAsync(newMovie);
            await _context.SaveChangesAsync();

            //Add Match Players
            foreach (var playerId in data.PlayerIds)
            {
                var newActorMovie = new Player_Match
                {
                    MatchId = newMovie.Id,
                    PlayerId = playerId
                };
                await _context.Players_Matches.AddAsync(newActorMovie);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Match> GetMatchByIdAsync(int id)
        {
            var movieDetails = await _context.Matches
                .Include(c => c.Team)
                .Include(p => p.Coach)
                .Include(am => am.Players_Matches).ThenInclude(a => a.Player)
                .FirstOrDefaultAsync(n => n.Id == id);

            return movieDetails;
        }

        public async Task<NewMatchDropdownsVM> GetNewMatchDropdownsValues()
        {
            var response = new NewMatchDropdownsVM
            {
                Players = await _context.Players.OrderBy(n => n.FullName).ToListAsync(),
                Teams = await _context.Teams.OrderBy(n => n.Name).ToListAsync(),
                Coaches = await _context.Coaches.OrderBy(n => n.FullName).ToListAsync()
            };

            return response;
        }

        public async Task UpdateMatchAsync(NewMatchVM data)
        {
            var dbMovie = await _context.Matches.FirstOrDefaultAsync(n => n.Id == data.Id);

            if (dbMovie != null)
            {
                dbMovie.Name = data.Name;
                dbMovie.Description = data.Description;
                dbMovie.Price = data.Price;
                dbMovie.ImageURL = data.ImageURL;
                dbMovie.TeamId = data.TeamId;
                dbMovie.StartDate = data.StartDate;
                dbMovie.EndDate = data.EndDate;
                dbMovie.MatchCategory = data.MatchCategory;
                dbMovie.CoachId = data.CoachId;
                await _context.SaveChangesAsync();
            }

            //Remove existing Players
            var existingActorsDb = _context.Players_Matches.Where(n => n.MatchId == data.Id).ToList();
            _context.Players_Matches.RemoveRange(existingActorsDb);
            await _context.SaveChangesAsync();

            //Add Match Players
            foreach (var playerIdId in data.PlayerIds)
            {
                var newActorMovie = new Player_Match
                {
                    MatchId = data.Id,
                    PlayerId = playerIdId
                };
                await _context.Players_Matches.AddAsync(newActorMovie);
            }

            await _context.SaveChangesAsync();
        }
    }
}
