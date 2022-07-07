using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace eTournament.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Full name")] public string FullName { get; set; }
    }
}