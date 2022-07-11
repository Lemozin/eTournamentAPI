using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eTournamentAPI.Data.Base;

namespace eTournamentAPI.Models
{
    [Table("EmailSMTPCredentials")]
    public class EmailSMTPCredentials : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
