using eTournamentAPI.Data.Base;
using eTournamentAPI.Models;

namespace eTournamentAPI.Data.Services
{
    public class TeamService : EntityBaseRepository<Team>, ITeamService
    {
        public TeamService(AppDbContext context) : base(context)
        {
        }
    }
}
