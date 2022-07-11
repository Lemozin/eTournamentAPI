using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTournamentAPI.Models;

[Table("Orders")]
public class Order
{
    [Key] public int Id { get; set; }

    public string Email { get; set; }

    public List<OrderItem> OrderItems { get; set; }
}