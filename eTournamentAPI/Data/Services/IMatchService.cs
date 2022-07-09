using eTournamentAPI.Data.Base;
using eTournamentAPI.Data.ViewModels;
using eTournamentAPI.Models;

namespace eTournamentAPI.Data.Services;

public interface IMatchService : IEntityBaseRepository<Match>
{
    Task<Match> GetMatchByIdAsync(int id);
    Task<NewMatchDropdownsVM> GetNewMatchDropdownsValues();
    Task AddNewMatchAsync(NewMatchVM data);
    Task UpdateMatchAsync(NewMatchVM data);
}