using Microsoft.EntityFrameworkCore;
using PersonService.Domain.Models;

namespace PersonService.Infrastructure.Contexts
{
    /// <summary>
    /// Db context class for the PersonDb database
    /// </summary>
    public class PersonDbContext : DbContext
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonDbContext"/> class.
        /// </summary>
        /// <param name="options">Db context options</param>
        public PersonDbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion

        #region Public properties

        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<SocialMediaAccount> SocialMediaAccounts { get; set; }
        public virtual DbSet<SocialSkill> SocialSkills { get; set; }

        #endregion

        #region Protected methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Persons", "PersonDb");

                entity.HasKey(e => e.PersonId);

                entity.Property(e => e.PersonId).ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName)
                    .IsRequired();

                entity.Property(e => e.LastName)
                    .IsRequired();
            });

            modelBuilder.Entity<SocialMediaAccount>(entity =>
            {
                entity.ToTable("SocialMediaAccounts", "PersonDb");

                entity.HasKey(e => e.SocialMediaAccountId);

                entity.Property(e => e.SocialMediaAccountId).ValueGeneratedOnAdd();

                entity.Property(e => e.Type)
                    .IsRequired();
            });

            modelBuilder.Entity<SocialSkill>(entity =>
            {
                entity.ToTable("SocialSkills", "PersonDb");

                entity.HasKey(e => e.SocialSkillId);

                entity.Property(e => e.SocialSkillId).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired();

                entity
                    .HasOne(ss => ss.Person)
                    .WithMany(p => p.SocialSkills);
            });

            modelBuilder.Entity<PersonSocialMediaAccount>(entity =>
            {
                entity.ToTable("PersonSocialMediaAccounts", "PersonDb");

                entity.HasKey(e => new { e.PersonId, e.SocialMediaAccountId });

                entity
                    .HasOne(psma => psma.Person)
                    .WithMany(p => p.PersonSocialMediaAccounts)
                    .HasForeignKey(psma => psma.PersonId);

                entity
                    .HasOne(psma => psma.SocialMediaAccount)
                    .WithMany(sma => sma.PersonSocialMediaAccounts)
                    .HasForeignKey(psma => psma.SocialMediaAccountId);

                entity.Property(e => e.Address)
                    .IsRequired();
            });
        }

        #endregion
    }
}
