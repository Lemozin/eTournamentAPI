using eTournamentAPI.Data.Enums;
using eTournamentAPI.Data.Static;
using eTournamentAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace eTournamentAPI.Data;

public class AppDbInitializer
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

            context.Database.EnsureCreated();

            //Team
            if (!context.Teams.Any())
            {
                context.Teams.AddRange(new List<Team>
                {
                    new()
                    {
                        Name = "Team 1",
                        Logo = "https://i.postimg.cc/BQnT6KNY/Team-1.jpg",
                        Description = "This is the description of the first team"
                    },
                    new()
                    {
                        Name = "Team 2",
                        Logo = "https://i.postimg.cc/VLK3rXwd/Team-2.jpg",
                        Description = "This is the description of the second team"
                    },
                    new()
                    {
                        Name = "Team 3",
                        Logo = "https://i.postimg.cc/Y9NPpW82/Team-3.jpg",
                        Description = "This is the description of the third team"
                    },
                    new()
                    {
                        Name = "Team 4",
                        Logo = "https://i.postimg.cc/GmhyL8JL/Team-4.jpg",
                        Description = "This is the description of the fourth team"
                    },
                    new()
                    {
                        Name = "Team 5",
                        Logo = "https://i.postimg.cc/XqSsmc57/Team-5.jpg",
                        Description = "This is the description of the fifth team"
                    }
                });
                context.SaveChanges();
            }

            //Players
            if (!context.Players.Any())
            {
                context.Players.AddRange(new List<Player>
                {
                    new()
                    {
                        FullName = "Player 1",
                        Bio = "This is the Bio of the first player",
                        ProfilePictureURL = "https://i.postimg.cc/R0ztfswR/player-1.jpg"
                    },
                    new()
                    {
                        FullName = "Player 2",
                        Bio = "This is the Bio of the second player",
                        ProfilePictureURL = "https://i.postimg.cc/0yGYt1k3/player-2.jpg"
                    },
                    new()
                    {
                        FullName = "Player 3",
                        Bio = "This is the Bio of the third player",
                        ProfilePictureURL = "https://i.postimg.cc/rsn46q0y/player-3.jpg"
                    },
                    new()
                    {
                        FullName = "Player 4",
                        Bio = "This is the Bio of the fourth player",
                        ProfilePictureURL = "https://i.postimg.cc/GpmHCWRH/player-4.jpg"
                    },
                    new()
                    {
                        FullName = "Player 5",
                        Bio = "This is the Bio of the fifth player",
                        ProfilePictureURL = "https://i.postimg.cc/1txhHFvs/player-5.jpg"
                    }
                });
                context.SaveChanges();
            }

            //Coaches
            if (!context.Coaches.Any())
            {
                context.Coaches.AddRange(new List<Coach>
                {
                    new()
                    {
                        FullName = "Coach 1",
                        Bio = "This is the Bio of the first coach",
                        ProfilePictureURL = "https://i.postimg.cc/rmLYn0Hk/User-1.jpg"
                    },
                    new()
                    {
                        FullName = "Coach 2",
                        Bio = "This is the Bio of the second coach",
                        ProfilePictureURL = "https://i.postimg.cc/c1f8GK5z/User-2.jpg"
                    },
                    new()
                    {
                        FullName = "Coach 3",
                        Bio = "This is the Bio of the third coach",
                        ProfilePictureURL = "https://i.postimg.cc/JzYQHD8q/User-3.jpg"
                    },
                    new()
                    {
                        FullName = "Coach 4",
                        Bio = "This is the Bio of the fourth coach",
                        ProfilePictureURL = "https://i.postimg.cc/c4HggDLp/User-4.jpg"
                    },
                    new()
                    {
                        FullName = "Coach 5",
                        Bio = "This is the Bio of the fifth coach",
                        ProfilePictureURL = "https://i.postimg.cc/2SdDzYfr/User-5.jpg"
                    }
                });
                context.SaveChanges();
            }

            //Matches
            if (!context.Matches.Any())
            {
                context.Matches.AddRange(new List<Match>
                {
                    new()
                    {
                        Name = "Champions League Final",
                        Description = "This is the Champions League Final description",
                        Price = 39.50,
                        ImageURL = "https://i.postimg.cc/pdbT2jX7/Match-1.jpg",
                        StartDate = DateTime.Now.AddDays(-10),
                        EndDate = DateTime.Now.AddDays(10),
                        TeamId = 3,
                        CoachId = 3,
                        MatchCategory = MatchCategory.Final
                    },
                    new()
                    {
                        Name = "Copa America Final",
                        Description = "This is the Copa America Final description",
                        Price = 29.50,
                        ImageURL = "https://i.postimg.cc/d31QWm47/Match-2.jpg",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(3),
                        TeamId = 1,
                        CoachId = 1,
                        MatchCategory = MatchCategory.Final
                    },
                    new()
                    {
                        Name = "Premier League",
                        Description = "This is the Premier League description",
                        Price = 39.50,
                        ImageURL = "https://i.postimg.cc/7L3LbL5L/Match-3.jpg",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(7),
                        TeamId = 4,
                        CoachId = 4,
                        MatchCategory = MatchCategory.Quarterfinals
                    },
                    new()
                    {
                        Name = "Europa League Final",
                        Description = "This is the Europa League Final description",
                        Price = 39.50,
                        ImageURL = "https://i.postimg.cc/W3s1ZPZw/Match-4.jpg",
                        StartDate = DateTime.Now.AddDays(-10),
                        EndDate = DateTime.Now.AddDays(-5),
                        TeamId = 1,
                        CoachId = 2,
                        MatchCategory = MatchCategory.Semifinals
                    },
                    new()
                    {
                        Name = "Euro 2022 Group Stage",
                        Description = "This is the Euro 2022 Group Stage description",
                        Price = 39.50,
                        ImageURL = "https://i.postimg.cc/rmgFCYCX/Match-5.jpg",
                        StartDate = DateTime.Now.AddDays(-10),
                        EndDate = DateTime.Now.AddDays(-2),
                        TeamId = 1,
                        CoachId = 3,
                        MatchCategory = MatchCategory.SecondRound
                    },
                    new()
                    {
                        Name = "Premier League - Fourth Round",
                        Description = "This is the Premier League - Fourth Round description",
                        Price = 39.50,
                        ImageURL = "https://i.postimg.cc/0y6fTTbn/Match-6.jpg",
                        StartDate = DateTime.Now.AddDays(3),
                        EndDate = DateTime.Now.AddDays(20),
                        TeamId = 1,
                        CoachId = 5,
                        MatchCategory = MatchCategory.FourthRound
                    }
                });
                context.SaveChanges();
            }

            //Players & Matches
            if (!context.Players_Matches.Any())
            {
                context.Players_Matches.AddRange(new List<Player_Match>
                {
                    new()
                    {
                        PlayerId = 1,
                        MatchId = 1
                    },
                    new()
                    {
                        PlayerId = 3,
                        MatchId = 1
                    },

                    new()
                    {
                        PlayerId = 1,
                        MatchId = 2
                    },
                    new()
                    {
                        PlayerId = 4,
                        MatchId = 2
                    },

                    new()
                    {
                        PlayerId = 1,
                        MatchId = 3
                    },
                    new()
                    {
                        PlayerId = 2,
                        MatchId = 3
                    },
                    new()
                    {
                        PlayerId = 5,
                        MatchId = 3
                    },


                    new()
                    {
                        PlayerId = 2,
                        MatchId = 4
                    },
                    new()
                    {
                        PlayerId = 3,
                        MatchId = 4
                    },
                    new()
                    {
                        PlayerId = 4,
                        MatchId = 4
                    },


                    new()
                    {
                        PlayerId = 2,
                        MatchId = 5
                    },
                    new()
                    {
                        PlayerId = 3,
                        MatchId = 5
                    },
                    new()
                    {
                        PlayerId = 4,
                        MatchId = 5
                    },
                    new()
                    {
                        PlayerId = 5,
                        MatchId = 5
                    },


                    new()
                    {
                        PlayerId = 3,
                        MatchId = 6
                    },
                    new()
                    {
                        PlayerId = 4,
                        MatchId = 6
                    },
                    new()
                    {
                        PlayerId = 5,
                        MatchId = 6
                    }
                });
                context.SaveChanges();
            }
        }
    }

    public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            //Roles
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            //Coaches
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var adminUserEmail = "admin@etournament.com";

            var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
            if (adminUser == null)
            {
                var newAdminUser = new ApplicationUser
                {
                    FullName = "Admin User",
                    UserName = "admin-user",
                    Email = adminUserEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
            }


            var appUserEmail = "user@etournament.com";

            var appUser = await userManager.FindByEmailAsync(appUserEmail);
            if (appUser == null)
            {
                var newAppUser = new ApplicationUser
                {
                    FullName = "Application User",
                    UserName = "app-user",
                    Email = appUserEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(newAppUser, "Coding@1234?");
                await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
            }
        }
    }
}