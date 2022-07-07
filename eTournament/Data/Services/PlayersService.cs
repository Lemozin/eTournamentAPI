using eTournament.Data.Base;
using eTournament.Models;

namespace eTournament.Data.Services
{
    public class PlayersService : EntityBaseRepository<Player>, IPlayersService
    {
        public PlayersService(AppDbContext context) : base(context)
        {
        }
    }
}