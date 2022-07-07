namespace eTournament.Models
{
    public class Player_Match
    {
        public int MatchId { get; set; }
        public Match Match { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }
    }
}