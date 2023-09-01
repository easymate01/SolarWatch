﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SolarWatch;

#nullable disable

namespace SolarWatch.Migrations
{
    [DbContext(typeof(SolarWatchApiContext))]
    [Migration("20230901112111_UpdateSSCityId")]
    partial class UpdateSSCityId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SolarWatch.Models.Cities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("SolarWatch.Models.SunriseSunset.SunriseSunsetResults", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTimeOffset>("Sunrise")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("Sunset")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("SunriseSunsetTimes");
                });

            modelBuilder.Entity("SolarWatch.Models.Cities.City", b =>
                {
                    b.OwnsOne("SolarWatch.Models.Coordinates", "Coordinates", b1 =>
                        {
                            b1.Property<int>("CityId")
                                .HasColumnType("int");

                            b1.Property<double>("Lat")
                                .HasColumnType("float")
                                .HasColumnName("Coordinates_Lat");

                            b1.Property<double>("Lon")
                                .HasColumnType("float")
                                .HasColumnName("Coordinates_Lon");

                            b1.HasKey("CityId");

                            b1.ToTable("Cities");

                            b1.WithOwner()
                                .HasForeignKey("CityId");
                        });

                    b.Navigation("Coordinates")
                        .IsRequired();
                });

            modelBuilder.Entity("SolarWatch.Models.SunriseSunset.SunriseSunsetResults", b =>
                {
                    b.HasOne("SolarWatch.Models.Cities.City", "City")
                        .WithOne("SunriseSunsetInfo")
                        .HasForeignKey("SolarWatch.Models.SunriseSunset.SunriseSunsetResults", "CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("SolarWatch.Models.Cities.City", b =>
                {
                    b.Navigation("SunriseSunsetInfo")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
