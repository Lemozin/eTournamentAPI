using eTournamentAPI.Data.Base;
using eTournamentAPI.Models;

namespace eTournamentAPI.Data.Services
{
    public interface IEmailService : IEntityBaseRepository<EmailSMTPCredentials>
    {
    }
}
