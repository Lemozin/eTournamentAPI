using eTournamentAPI.Data.Base;
using eTournamentAPI.Models;

namespace eTournamentAPI.Data.Services
{
    public class UserService : EntityBaseRepository<Coach>, IUserService
    {
        public UserService(AppDbContext context) : base(context)
        {
        }
    }
}
