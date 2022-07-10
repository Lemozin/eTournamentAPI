using eTournamentAPI.Models;

namespace eTournament.Data.RequestReturnModels;

public class RequestEditPlayerModel
{
    public int id { get; set; }
    public Player Player { get; set; }
}