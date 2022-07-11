using eTournamentAPI.Models;
using eTournamentAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eTournamentAPI.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Player_Match> Players_Matches { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<EmailSMTPCredentials> EmailSmtpCredentials { get; set; }


    //Orders related tables
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player_Match>().HasKey(am => new
        {
            am.PlayerId,
            am.MatchId
        });

        modelBuilder.Entity<Player_Match>().HasOne(m => m.Match).WithMany(am => am.Players_Matches)
            .HasForeignKey(m => m.MatchId);
        modelBuilder.Entity<Player_Match>().HasOne(m => m.Player).WithMany(am => am.Players_Matches)
            .HasForeignKey(m => m.PlayerId);


        base.OnModelCreating(modelBuilder);
    }
}