using eTournament.Data.Base;
using eTournament.Models;

namespace eTournament.Data.Services
{
    public class UserService : EntityBaseRepository<Coach>, IUserService
    {
        public UserService(AppDbContext context) : base(context)
        {
        }
    }
}