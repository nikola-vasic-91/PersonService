﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonService.Infrastructure.Contexts;

#nullable disable

namespace PersonService.Infrastructure.Migrations
{
    [DbContext(typeof(PersonDbContext))]
    [Migration("20220919101834_Initial_Setup")]
    partial class Initial_Setup
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PersonService.Domain.Models.Person", b =>
                {
                    b.Property<Guid>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PersonId");

                    b.ToTable("Persons", "PersonDb");
                });

            modelBuilder.Entity("PersonService.Domain.Models.PersonSocialMediaAccount", b =>
                {
                    b.Property<Guid>("PersonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SocialMediaAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PersonId", "SocialMediaAccountId");

                    b.HasIndex("SocialMediaAccountId");

                    b.ToTable("PersonSocialMediaAccounts", "PersonDb");
                });

            modelBuilder.Entity("PersonService.Domain.Models.SocialMediaAccount", b =>
                {
                    b.Property<Guid>("SocialMediaAccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SocialMediaAccountId");

                    b.ToTable("SocialMediaAccounts", "PersonDb");
                });

            modelBuilder.Entity("PersonService.Domain.Models.SocialSkill", b =>
                {
                    b.Property<Guid>("SocialSkillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("SocialSkillId");

                    b.HasIndex("PersonId");

                    b.ToTable("SocialSkills", "PersonDb");
                });

            modelBuilder.Entity("PersonService.Domain.Models.PersonSocialMediaAccount", b =>
                {
                    b.HasOne("PersonService.Domain.Models.Person", "Person")
                        .WithMany("PersonSocialMediaAccounts")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PersonService.Domain.Models.SocialMediaAccount", "SocialMediaAccount")
                        .WithMany("PersonSocialMediaAccounts")
                        .HasForeignKey("SocialMediaAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("SocialMediaAccount");
                });

            modelBuilder.Entity("PersonService.Domain.Models.SocialSkill", b =>
                {
                    b.HasOne("PersonService.Domain.Models.Person", "Person")
                        .WithMany("SocialSkills")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("PersonService.Domain.Models.Person", b =>
                {
                    b.Navigation("PersonSocialMediaAccounts");

                    b.Navigation("SocialSkills");
                });

            modelBuilder.Entity("PersonService.Domain.Models.SocialMediaAccount", b =>
                {
                    b.Navigation("PersonSocialMediaAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}
