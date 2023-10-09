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
    [Migration("20230915075446_HasChanged_to_HasBeenSent")]
    partial class HasChanged_to_HasBeenSent
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

                    b.Property<string>("RID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.ToTable("AlarmC");
                });

            modelBuilder.Entity("Core.Entities.AlarmsLog.Models.DB.AlarmLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("AlarmID")
                        .HasColumnType("int");

                    b.Property<TimeSpan?>("Duration")
                        .HasColumnType("time");

                    b.Property<bool>("HasBeenSent")
                        .HasColumnType("bit");

                    b.Property<string>("IRID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAck")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Station")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("TSClear")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("TSGet")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("TSRaised")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("TSRead")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.HasIndex("AlarmID");

                    b.ToTable("AlarmLog");
                });

            modelBuilder.Entity("Core.Entities.AlarmsPLC.Models.DB.AlarmPLC", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("AlarmID")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

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

                    b.Property<int>("AlarmID")
                        .HasColumnType("int");

                    b.Property<string>("IRID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("NbNonAck")
                        .HasColumnType("int");

                    b.Property<string>("Station")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("TSClear")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("TSRaised")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.HasIndex("AlarmID")
                        .IsUnique();

                    b.ToTable("AlarmRT");
                });

            modelBuilder.Entity("Core.Entities.AlarmsLog.Models.DB.AlarmLog", b =>
                {
                    b.HasOne("Core.Entities.AlarmsC.Models.DB.AlarmC", "Alarm")
                        .WithMany("AlarmLogs")
                        .HasForeignKey("AlarmID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Alarm");
                });

            modelBuilder.Entity("Core.Entities.AlarmsRT.Models.DB.AlarmRT", b =>
                {
                    b.HasOne("Core.Entities.AlarmsC.Models.DB.AlarmC", "Alarm")
                        .WithOne("AlarmRT")
                        .HasForeignKey("Core.Entities.AlarmsRT.Models.DB.AlarmRT", "AlarmID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Alarm");
                });

            modelBuilder.Entity("Core.Entities.AlarmsC.Models.DB.AlarmC", b =>
                {
                    b.Navigation("AlarmLogs");

                    b.Navigation("AlarmRT")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
