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
    [Migration("20230912152212_Refactor_Name_Convention")]
    partial class Refactor_Name_Convention
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Core.Entities.AlarmsC.Models.DB.AlarmC", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.ToTable("AlarmC");
                });

            modelBuilder.Entity("Core.Entities.AlarmsPLC.Models.DB.AlarmPLC", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("IDAlarm")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.ToTable("AlarmPLC");
                });

            modelBuilder.Entity("Core.Entities.AlarmsRT.Models.DB.AlarmRT", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("IDAlarm")
                        .HasColumnType("int");

                    b.Property<int?>("NumberNonRead")
                        .HasColumnType("int");

                    b.Property<string>("Station")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.HasIndex("IDAlarm")
                        .IsUnique();

                    b.ToTable("AlarmRT");
                });

            modelBuilder.Entity("Core.Entities.Journals.Models.DB.Journal", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("IDAlarm")
                        .HasColumnType("int");

                    b.Property<int?>("Read")
                        .HasColumnType("int");

                    b.Property<string>("Station")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Status0")
                        .HasColumnType("int");

                    b.Property<int?>("Status1")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("TS0")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("TS1")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("TSRead")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.HasIndex("IDAlarm");

                    b.ToTable("Journal");
                });

            modelBuilder.Entity("Core.Entities.AlarmsRT.Models.DB.AlarmRT", b =>
                {
                    b.HasOne("Core.Entities.AlarmsC.Models.DB.AlarmC", "AlarmC")
                        .WithOne("AlarmRT")
                        .HasForeignKey("Core.Entities.AlarmsRT.Models.DB.AlarmRT", "IDAlarm")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AlarmC");
                });

            modelBuilder.Entity("Core.Entities.Journals.Models.DB.Journal", b =>
                {
                    b.HasOne("Core.Entities.AlarmsC.Models.DB.AlarmC", "Alarm")
                        .WithMany("Journals")
                        .HasForeignKey("IDAlarm")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Alarm");
                });

            modelBuilder.Entity("Core.Entities.AlarmsC.Models.DB.AlarmC", b =>
                {
                    b.Navigation("AlarmRT")
                        .IsRequired();

                    b.Navigation("Journals");
                });
#pragma warning restore 612, 618
        }
    }
}
