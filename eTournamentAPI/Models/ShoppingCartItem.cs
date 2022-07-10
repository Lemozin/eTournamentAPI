using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTournamentAPI.Models;

[Table("ShoppingCartItemes")]
public class ShoppingCartItem
{
    [Key] public int Id { get; set; }

    public Match Match { get; set; }
    public int Amount { get; set; }


    public string ShoppingCartId { get; set; }
    public int Status { get; set; }
}