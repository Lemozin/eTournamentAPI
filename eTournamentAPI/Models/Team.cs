using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eTournamentAPI.Data.Base;

namespace eTournamentAPI.Models;

[Table("Teams")]
public class Team : IEntityBase
{
    [Display(Name = "Team Logo")]
    [Required(ErrorMessage = "Team logo is required")]
    public string Logo { get; set; }

    [Display(Name = "Team Name")]
    [Required(ErrorMessage = "Team name is required")]
    public string Name { get; set; }

    [Display(Name = "Description")]
    [Required(ErrorMessage = "Team description is required")]
    public string Description { get; set; }

    //Relationships
    public List<Match> Matches { get; set; }

    [Key] public int Id { get; set; }
}