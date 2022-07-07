using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eTournamentAPI.Data.Enums;

namespace eTournamentAPI.Data.ViewModels
{
    public class NewMatchVM
    {
        public int Id { get; set; }

        [Display(Name = "Match name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "Match description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Display(Name = "Price in R")]
        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        [Display(Name = "Match poster URL")]
        [Required(ErrorMessage = "Match poster URL is required")]
        public string ImageURL { get; set; }

        [Display(Name = "Match start date")]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Match end date")]
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Select a category")]
        [Required(ErrorMessage = "Match category is required")]
        public MatchCategory MatchCategory { get; set; }

        //Relationships
        [Display(Name = "Select player(s)")]
        [Required(ErrorMessage = "Match player(s) is required")]
        public List<int> PlayerIds { get; set; }

        [Display(Name = "Select a Team")]
        [Required(ErrorMessage = "Match Team is required")]
        public int TeamId { get; set; }

        [Display(Name = "Select a Coach")]
        [Required(ErrorMessage = "Match Coach is required")]
        public int CoachId { get; set; }
    }
}
