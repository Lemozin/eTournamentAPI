using System.Collections.Generic;
using eTournament.Models;

namespace eTournament.Data.ViewModels
{
    public class NewMatchDropdownsVM
    {
        public NewMatchDropdownsVM()
        {
            Coaches = new List<Coach>();
            Teams = new List<Team>();
            Players = new List<Player>();
        }

        public List<Coach> Coaches { get; set; }
        public List<Team> Teams { get; set; }
        public List<Player> Players { get; set; }
    }
}