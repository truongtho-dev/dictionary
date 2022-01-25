using Dictionary.Models;
using Dictionary.Models.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DictionaryEntity = Dictionary.Models.Entities.Dictionary;

namespace Dictionary.Seeders
{
    public class Seeder
    {
        private const string DefaultPassword = "123456";

        public static void SeedTestData(ApplicationDbContext context)
        {
            SeedUserAndRoles(context).Wait();
            SeedDictionaryData(context).Wait();
        }

        private static async Task SeedUserAndRoles(ApplicationDbContext context)
        {
            Console.WriteLine(@"Seeding user and roles");

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new ApplicationUserManager(userStore);

            var roles = new [] { "guest", "admin" };

            foreach (var role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role))
                {
                    await roleStore.CreateAsync(new IdentityRole(role));
                }
            }

            await context.SaveChangesAsync();

            var guestUser = new ApplicationUser
            {
                Email = "guest@test.com",
                UserName = "guest",
                EmailConfirmed = true,
                LockoutEnabled = false,
            };

            var adminUser = new ApplicationUser
            {
                Email = "admin@test.com",
                UserName = "admin",
                EmailConfirmed = true,
                LockoutEnabled = false,
            };

            var userSeeds = new [] { guestUser, adminUser };

            foreach (var seedUser in userSeeds)
            {
                if (!await context.Users.AnyAsync(u => u.Email == seedUser.Email))
                {
                    Console.WriteLine($@"Add user: {seedUser.UserName}");
                    var result = await userManager.CreateAsync(seedUser, DefaultPassword);
                    if (result.Succeeded)
                    {
                        Console.WriteLine($@"Assign role [{seedUser.UserName}] to {seedUser.UserName}");
                        await userManager.AddToRoleAsync(seedUser.Id, seedUser.UserName); // Small trick, role = username
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedDictionaryData(ApplicationDbContext context)
        {
            Console.WriteLine(@"Seeding dictionary data");

            var vnLang = new Language
            {
                Code = "vn",
                Name = "Vietnamese",
            };
            var enLang = new Language
            {
                Code = "en",
                Name = "English",
            };
            var languageSeeds = new [] { vnLang, enLang };

            foreach (var languageSeed in languageSeeds)
            {
                if (!context.Languages.Any(l => l.Code == languageSeed.Code))
                {
                    context.Languages.Add(languageSeed);
                }
            }

            var enVnDict = new DictionaryEntity
            {
                Source = enLang,
                Destination = vnLang,
                Name = "English - Vietnamese",
                Entries = new List<DictionaryEntry>
                {
                    new DictionaryEntry
                    {
                        Word = "apple",
                        Meaning = "trái táo, một loại trái cây",
                    },
                },
            };

            var vnEnDict = new DictionaryEntity
            {
                Source = vnLang,
                Destination = enLang,
                Name = "Vietnamese - English",
                Entries = new List<DictionaryEntry>
                {
                    new DictionaryEntry
                    {
                        Word = "táo",
                        Meaning = "apple, a kind of fruit",
                    },
                },
            };
            var dictSeeds = new [] { enVnDict, vnEnDict };

            foreach (var dictSeed in dictSeeds)
            {
                if (!context.Dictionaries.Any(d => 
                    d.Source.Code == dictSeed.Source.Code &&
                    d.Destination.Code == dictSeed.Destination.Code))
                {
                    context.Dictionaries.Add(dictSeed);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}