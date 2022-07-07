using eTournamentAPI.Data.Base;
using eTournamentAPI.Models;

namespace eTournamentAPI.Data.Services
{
    public class PlayersService : EntityBaseRepository<Player>, IPlayersService
    {
        public PlayersService(AppDbContext context) : base(context)
        {
        }
    }
}
