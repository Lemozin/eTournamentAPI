using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eTournamentAPI.Data.Base;
using eTournamentAPI.Data.Enums;

namespace eTournamentAPI.Models;

[Table("Matches")]
public class Match : IEntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string ImageURL { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public MatchCategory MatchCategory { get; set; }

    //Relationships
    public List<Player_Match> Players_Matches { get; set; }

    //Team
    public int TeamId { get; set; }

    [ForeignKey("TeamId")] public Team Team { get; set; }

    //Coach
    public int CoachId { get; set; }

    [ForeignKey("CoachId")] public Coach Coach { get; set; }

    [Key] public int Id { get; set; }
}