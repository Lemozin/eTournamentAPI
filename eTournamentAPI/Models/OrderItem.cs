using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTournamentAPI.Models
{
    [Table("OrderItemes")]
    public class OrderItem
    {
        [Key] public int Id { get; set; }

        public int Amount { get; set; }
        public double Price { get; set; }

        public int MatchId { get; set; }

        [ForeignKey("MatchId")] public Match Match { get; set; }

        public int OrderId { get; set; }

        [ForeignKey("OrderId")] public Order Order { get; set; }
    }
}
