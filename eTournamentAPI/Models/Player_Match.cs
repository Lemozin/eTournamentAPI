using System.ComponentModel.DataAnnotations.Schema;

namespace eTournamentAPI.Models;

[Table("Players_Matches")]
public class Player_Match
{
    public int MatchId { get; set; }
    public Match Match { get; set; }

    public int PlayerId { get; set; }
    public Player Player { get; set; }
}