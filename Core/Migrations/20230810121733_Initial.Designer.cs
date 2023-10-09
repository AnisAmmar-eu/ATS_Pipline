﻿// <auto-generated />
using System;
using Core.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Core.Migrations
{
    [DbContext(typeof(AnodeCTX))]
    [Migration("20230810121733_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Core.Entities.Alarmes_C.Models.DB.Alarme_C", b =>
                {
                    b.Property<int>("IdAlarm")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAlarm"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdAlarm");

                    b.ToTable("Alarme_C");
                });

            modelBuilder.Entity("Core.Entities.AlarmesPLC.Models.DB.AlarmePLC", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("IdAlarme")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("AlarmePLC");
                });

            modelBuilder.Entity("Core.Entities.AlarmesTR.Models.DB.AlarmeTR", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("IdAlarme")
                        .HasColumnType("int");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("TS")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("IdAlarme")
                        .IsUnique();

                    b.ToTable("AlarmeTR");
                });

            modelBuilder.Entity("Core.Entities.Journals.Models.DB.Journal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("IdAlarme")
                        .HasColumnType("int");

                    b.Property<int?>("Status0")
                        .HasColumnType("int");

                    b.Property<int?>("Status1")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("TS")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("TS0")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("TS1")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("IdAlarme");

                    b.ToTable("Journal");
                });

            modelBuilder.Entity("Core.Entities.AlarmesTR.Models.DB.AlarmeTR", b =>
                {
                    b.HasOne("Core.Entities.Alarmes_C.Models.DB.Alarme_C", "Alarme_C")
                        .WithOne("AlarmeTR")
                        .HasForeignKey("Core.Entities.AlarmesTR.Models.DB.AlarmeTR", "IdAlarme")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Alarme_C");
                });

            modelBuilder.Entity("Core.Entities.Journals.Models.DB.Journal", b =>
                {
                    b.HasOne("Core.Entities.Alarmes_C.Models.DB.Alarme_C", "Alarme")
                        .WithMany("Journaux")
                        .HasForeignKey("IdAlarme")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Alarme");
                });

            modelBuilder.Entity("Core.Entities.Alarmes_C.Models.DB.Alarme_C", b =>
                {
                    b.Navigation("AlarmeTR")
                        .IsRequired();

                    b.Navigation("Journaux");
                });
#pragma warning restore 612, 618
        }
    }
}
