using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eTournament.Data.Base;

namespace eTournament.Models
{
    [Table("EmailSMTPCredentials")]
    public class EmailSMTPCredentials : IEntityBase
    {
        public int Port { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        [Key] public int Id { get; set; }
    }
}