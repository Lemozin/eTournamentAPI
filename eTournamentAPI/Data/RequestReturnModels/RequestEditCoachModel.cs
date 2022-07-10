using eTournamentAPI.Models;

namespace eTournament.Data.RequestReturnModels;

public class RequestEditCoachModel
{
    public int id { get; set; }
    public Coach Coach { get; set; }
}