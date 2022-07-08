using Microsoft.Extensions.Configuration;

namespace eTournamentAPI
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string username);
    }
}
