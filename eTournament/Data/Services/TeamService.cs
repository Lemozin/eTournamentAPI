using eTournament.Data.Base;
using eTournament.Models;

namespace eTournament.Data.Services
{
    public class TeamService : EntityBaseRepository<Team>, ITeamService
    {
        public TeamService(AppDbContext context) : base(context)
        {
        }
    }
}