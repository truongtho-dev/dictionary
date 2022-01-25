using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Text;
using Dictionary.Models.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dictionary.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("Database", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Language>()
                .HasMany(x => x.SourceDictionaries)
                .WithRequired(x => x.Source)
                .HasForeignKey(x => x.SourceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Language>()
                .HasMany(x => x.DestinationDictionaries)
                .WithRequired(x => x.Destination)
                .HasForeignKey(x => x.DestinationId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Entities.Dictionary>()
                .HasMany(x => x.Entries)
                .WithRequired(x => x.Dictionary)
                .HasForeignKey(x => x.DictionaryId);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException("Entity Validation Failed - errors follow:\n" + sb.ToString(), ex);
            }
        }

        public DbSet<Entities.Language> Languages { get; set; }
        public DbSet<Entities.Dictionary> Dictionaries { get; set; }
        public DbSet<Entities.DictionaryEntry> DictionaryEntries { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}