using System.Threading.Tasks;
using eTournament.Data.Base;
using eTournament.Data.ViewModels;
using eTournament.Models;

namespace eTournament.Data.Services
{
    public interface IMatchService : IEntityBaseRepository<Match>
    {
        Task<Match> GetMatchByIdAsync(int id);
        Task<NewMatchDropdownsVM> GetNewMatchDropdownsValues();
        Task AddNewMatchAsync(NewMatchVM data);
        Task UpdateMatchAsync(NewMatchVM data);
    }
}