using System;
using System.Text;
using System.Text.Json.Serialization;
using eTournamentAPI.Data;
using eTournamentAPI.Data.Cart;
using eTournamentAPI.Data.Services;
using eTournamentAPI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace eTournamentAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //DbContext configuration
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));

            //Services configuration
            services.AddScoped<IPlayersService, PlayersService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IOrdersService, OrdersService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(sc => ShoppingCart.GetShoppingCart(sc));

            //Authentication and authorization
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes((Configuration["Jwt:Key"])))
                };
            });
            services.AddMvc();
            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "eTournamentAPI", 
                    Version = "v1",
                    Description = "Basic e-commerce web api site",
                    Contact = new OpenApiContact()
                    {
                        Name = "Leonard Masina",
                        Email = "molemo.masina@gmail.com",
                        Url = new Uri("https://github.com/Lemozin/eTournamentAPI/tree/master/eTournamentAPI")
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var url = Configuration.GetValue<string>("Base:BaseUrl");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basic e-commerce web api site"));
            }

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url + "/swagger/v1/swagger.json", "Basic e-commerce web api site");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { 
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            //Seed database
            AppDbInitializer.Seed(app);
            AppDbInitializer.SeedUsersAndRolesAsync(app).Wait();
        }
    }
}