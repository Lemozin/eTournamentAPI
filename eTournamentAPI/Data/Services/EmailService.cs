using eTournamentAPI.Data.Base;
using eTournamentAPI.Models;

namespace eTournamentAPI.Data.Services;

public class EmailService : EntityBaseRepository<EmailSMTPCredentials>, IEmailService
{
    public EmailService(AppDbContext context) : base(context)
    {
    }
}