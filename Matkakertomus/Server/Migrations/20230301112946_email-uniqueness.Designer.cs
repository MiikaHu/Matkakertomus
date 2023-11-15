﻿// <auto-generated />
using System;
using Matkakertomus.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Matkakertomus.Server.Migrations
{
    [DbContext(typeof(TravelContext))]
    [Migration("20230301112946_email-uniqueness")]
    partial class emailuniqueness
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.3");

            modelBuilder.Entity("Matkakertomus.Server.Models.Destination", b =>
                {
                    b.Property<uint>("DestinationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("idmatkakohde");

                    b.Property<string>("Area")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT")
                        .HasColumnName("paikkakunta");

                    b.Property<string>("Country")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT")
                        .HasColumnName("maa");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT")
                        .HasColumnName("kuvausteksti");

                    b.Property<string>("DestinationName")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT")
                        .HasColumnName("kohdenimi");

                    b.Property<string>("Picture")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT")
                        .HasColumnName("kuva");

                    b.HasKey("DestinationId")
                        .HasName("PRIMARY");

                    b.ToTable("matkakohde", (string)null);
                });

            modelBuilder.Entity("Matkakertomus.Server.Models.Picture", b =>
                {
                    b.Property<uint>("PictureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("idkuva");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("TEXT")
                        .HasColumnName("kuva");

                    b.Property<uint>("StoryId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("idtarina");

                    b.HasKey("PictureId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "StoryId" }, "fk_kuva_tarina1_idx");

                    b.ToTable("kuva", (string)null);
                });

            modelBuilder.Entity("Matkakertomus.Server.Models.Story", b =>
                {
                    b.Property<uint>("StoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("idtarina");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date")
                        .HasColumnName("pvm");

                    b.Property<uint>("DestinationId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("idmatkakohde");

                    b.Property<string>("Text")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT")
                        .HasColumnName("teksti");

                    b.Property<uint>("TripId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("idmatka");

                    b.HasKey("StoryId")
                        .HasName("PRIMARY");

                    b.HasIndex("TripId");

                    b.HasIndex(new[] { "StoryId" }, "fk_tarina_matka1_idx");

                    b.HasIndex(new[] { "DestinationId" }, "fk_tarina_matkakohde1_idx");

                    b.ToTable("tarina", (string)null);
                });

            modelBuilder.Entity("Matkakertomus.Server.Models.Traveller", b =>
                {
                    b.Property<uint>("TravellerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("idmatkaaja");

                    b.Property<string>("Area")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT")
                        .HasColumnName("paikkakunta");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT")
                        .HasColumnName("esittely");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("TEXT")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("TEXT")
                        .HasColumnName("etunimi");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("TEXT")
                        .HasColumnName("sukunimi");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("BLOB")
                        .HasColumnName("passwordhash");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("BLOB")
                        .HasColumnName("passwordSalt");

                    b.Property<string>("Picture")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT")
                        .HasColumnName("kuva");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("TEXT")
                        .HasColumnName("nimimerkki");

                    b.HasKey("TravellerId")
                        .HasName("PRIMARY");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("matkaaja", (string)null);
                });

            modelBuilder.Entity("Matkakertomus.Server.Models.Trip", b =>
                {
                    b.Property<uint>("TripId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("idmatka");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("date")
                        .HasColumnName("loppupvm");

                    b.Property<sbyte?>("Private")
                        .HasColumnType("INTEGER")
                        .HasColumnName("yksityinen");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("date")
                        .HasColumnName("alkupvm");

                    b.Property<uint>("TravellerId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("idmatkaaja");

                    b.HasKey("TripId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "TravellerId" }, "fk_matkaaja_has_matkakohde_matkaaja_idx");

                    b.ToTable("matka", (string)null);
                });

            modelBuilder.Entity("Matkakertomus.Server.Models.Picture", b =>
                {
                    b.HasOne("Matkakertomus.Server.Models.Story", "Story")
                        .WithMany("Pictures")
                        .HasForeignKey("StoryId")
                        .IsRequired()
                        .HasConstraintName("fk_kuva_tarina1");

                    b.Navigation("Story");
                });

            modelBuilder.Entity("Matkakertomus.Server.Models.Story", b =>
                {
                    b.HasOne("Matkakertomus.Server.Models.Destination", "Destination")
                        .WithMany("Stories")
                        .HasForeignKey("DestinationId")
                        .IsRequired()
                        .HasConstraintName("fk_tarina_matkakohde1");

                    b.HasOne("Matkakertomus.Server.Models.Trip", "Trip")
                        .WithMany("Stories")
                        .HasForeignKey("TripId")
                        .IsRequired()
                        .HasConstraintName("fk_tarina_matka1");

                    b.Navigation("Destination");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("Matkakertomus.Server.Models.Trip", b =>
                {
                    b.HasOne("Matkakertomus.Server.Models.Traveller", "Traveller")
                        .WithMany("Trips")
                        .HasForeignKey("TravellerId")
                        .IsRequired()
                        .HasConstraintName("fk_matkaaja_has_matkakohde_matkaaja");

                    b.Navigation("Traveller");
                });

            modelBuilder.Entity("Matkakertomus.Server.Models.Destination", b =>
                {
                    b.Navigation("Stories");
                });

            modelBuilder.Entity("Matkakertomus.Server.Models.Story", b =>
                {
                    b.Navigation("Pictures");
                });

            modelBuilder.Entity("Matkakertomus.Server.Models.Traveller", b =>
                {
                    b.Navigation("Trips");
                });

            modelBuilder.Entity("Matkakertomus.Server.Models.Trip", b =>
                {
                    b.Navigation("Stories");
                });
#pragma warning restore 612, 618
        }
    }
}
