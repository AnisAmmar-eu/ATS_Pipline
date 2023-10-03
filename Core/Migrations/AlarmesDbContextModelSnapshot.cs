﻿// <auto-generated />
using System;
using Core.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Core.Migrations
{
    [DbContext(typeof(AlarmCTX))]
    partial class AlarmesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Core.Entities.Alarms.AlarmsC.Models.DB.AlarmC", b =>
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

            modelBuilder.Entity("Core.Entities.Alarms.AlarmsCycle.Models.DB.AlarmCycle", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("AlarmListPacketID")
                        .HasColumnType("int");

                    b.Property<string>("AlarmRID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("NbNonAck")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.HasIndex("AlarmListPacketID");

                    b.ToTable("AlarmCycle");
                });

            modelBuilder.Entity("Core.Entities.Alarms.AlarmsLog.Models.DB.AlarmLog", b =>
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

            modelBuilder.Entity("Core.Entities.Alarms.AlarmsPLC.Models.DB.AlarmPLC", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("AlarmID")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOneShot")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.ToTable("AlarmPLC");
                });

            modelBuilder.Entity("Core.Entities.Alarms.AlarmsRT.Models.DB.AlarmRT", b =>
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

                    b.Property<DateTimeOffset?>("TSClear")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("TSRaised")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.HasIndex("AlarmID")
                        .IsUnique();

                    b.ToTable("AlarmRT");
                });

            modelBuilder.Entity("Core.Entities.ExtTags.Models.DB.ExtTag", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("ServiceID")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.HasIndex("ServiceID");

                    b.ToTable("ExtTag");
                });

            modelBuilder.Entity("Core.Entities.Packets.Models.DB.Packet", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("CycleStationRID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Packet");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Packet");
                });

            modelBuilder.Entity("Core.Entities.ServicesMonitors.Models.DB.ServicesMonitor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.ToTable("ServicesMonitor");
                });

            modelBuilder.Entity("Core.Shared.Models.DB.System.Logs.Log", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Api")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Code")
                        .HasColumnType("int");

                    b.Property<string>("Controller")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Endpoint")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Function")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Server")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("Core.Entities.Packets.Models.DB.AlarmLists.AlarmList", b =>
                {
                    b.HasBaseType("Core.Entities.Packets.Models.DB.Packet");

                    b.HasDiscriminator().HasValue("AlarmList");
                });

            modelBuilder.Entity("Core.Entities.Packets.Models.DB.Detections.Detection", b =>
                {
                    b.HasBaseType("Core.Entities.Packets.Models.DB.Packet");

                    b.HasDiscriminator().HasValue("Detection");
                });

            modelBuilder.Entity("Core.Entities.Alarms.AlarmsCycle.Models.DB.AlarmCycle", b =>
                {
                    b.HasOne("Core.Entities.Packets.Models.DB.AlarmLists.AlarmList", "AlarmList")
                        .WithMany("AlarmCycles")
                        .HasForeignKey("AlarmListPacketID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AlarmList");
                });

            modelBuilder.Entity("Core.Entities.Alarms.AlarmsLog.Models.DB.AlarmLog", b =>
                {
                    b.HasOne("Core.Entities.Alarms.AlarmsC.Models.DB.AlarmC", "Alarm")
                        .WithMany("AlarmLogs")
                        .HasForeignKey("AlarmID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Alarm");
                });

            modelBuilder.Entity("Core.Entities.Alarms.AlarmsRT.Models.DB.AlarmRT", b =>
                {
                    b.HasOne("Core.Entities.Alarms.AlarmsC.Models.DB.AlarmC", "Alarm")
                        .WithOne("AlarmRT")
                        .HasForeignKey("Core.Entities.Alarms.AlarmsRT.Models.DB.AlarmRT", "AlarmID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Alarm");
                });

            modelBuilder.Entity("Core.Entities.ExtTags.Models.DB.ExtTag", b =>
                {
                    b.HasOne("Core.Entities.ServicesMonitors.Models.DB.ServicesMonitor", "Service")
                        .WithMany("ExtTags")
                        .HasForeignKey("ServiceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");
                });

            modelBuilder.Entity("Core.Entities.Alarms.AlarmsC.Models.DB.AlarmC", b =>
                {
                    b.Navigation("AlarmLogs");

                    b.Navigation("AlarmRT")
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.ServicesMonitors.Models.DB.ServicesMonitor", b =>
                {
                    b.Navigation("ExtTags");
                });

            modelBuilder.Entity("Core.Entities.Packets.Models.DB.AlarmLists.AlarmList", b =>
                {
                    b.Navigation("AlarmCycles");
                });
#pragma warning restore 612, 618
        }
    }
}
