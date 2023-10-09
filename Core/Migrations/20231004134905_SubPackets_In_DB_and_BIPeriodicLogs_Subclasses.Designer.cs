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
    [Migration("20231004134905_SubPackets_In_DB_and_BIPeriodicLogs_Subclasses")]
    partial class SubPackets_In_DB_and_BIPeriodicLogs_Subclasses
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("Core.Entities.BIPeriodicLogs.Models.DB.BIPeriodicLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("ActiveAlarms")
                        .HasColumnType("int");

                    b.Property<int>("Cam1Matched")
                        .HasColumnType("int");

                    b.Property<int>("Cam2Matched")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InactiveAlarms")
                        .HasColumnType("int");

                    b.Property<int>("NbAnodeS1")
                        .HasColumnType("int");

                    b.Property<int>("NbAnodeS2")
                        .HasColumnType("int");

                    b.Property<int>("NbAnodeS3")
                        .HasColumnType("int");

                    b.Property<int>("NbAnodeS4")
                        .HasColumnType("int");

                    b.Property<int>("NbAnodeS5")
                        .HasColumnType("int");

                    b.Property<int>("NbMatched")
                        .HasColumnType("int");

                    b.Property<int>("NbSigned")
                        .HasColumnType("int");

                    b.Property<int>("NbUnsigned")
                        .HasColumnType("int");

                    b.Property<int>("NonAckAlarms")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.ToTable("BIPeriodicLog");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BIPeriodicLog");
                });

            modelBuilder.Entity("Core.Entities.ExtTags.Models.DB.ExtTag", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("CurrentValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasNewValue")
                        .HasColumnType("bit");

                    b.Property<bool>("IsReadOnly")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ServiceID")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ValueType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IPAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsConnected")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.ToTable("ServicesMonitor");
                });

            modelBuilder.Entity("Core.Entities.User.Models.DB.Acts.Act", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("EntityType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.ToTable("Acts");
                });

            modelBuilder.Entity("Core.Entities.User.Models.DB.Acts.ActEntities.ActEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("ActID")
                        .HasColumnType("int");

                    b.Property<int?>("EntityID")
                        .HasColumnType("int");

                    b.Property<int?>("ParentID")
                        .HasColumnType("int");

                    b.Property<string>("RID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SignatureType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.HasIndex("ActID");

                    b.ToTable("ActEntities");
                });

            modelBuilder.Entity("Core.Entities.User.Models.DB.Acts.ActEntities.ActEntityRoles.ActEntityRole", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("ActEntityID")
                        .HasColumnType("int");

                    b.Property<string>("ApplicationID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApplicationType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Level")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("TS")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.HasIndex("ActEntityID");

                    b.ToTable("ActEntityRoles");
                });

            modelBuilder.Entity("Core.Entities.User.Models.DB.Roles.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Core.Entities.User.Models.DB.Users.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Firstname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEkium")
                        .HasColumnType("bit");

                    b.Property<string>("Lastname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Core.Entities.BIPeriodicLogs.Models.DB.Entries.AnnualEntries.AnnualEntry", b =>
                {
                    b.HasBaseType("Core.Entities.BIPeriodicLogs.Models.DB.BIPeriodicLog");

                    b.HasDiscriminator().HasValue("AnnualEntry");
                });

            modelBuilder.Entity("Core.Entities.BIPeriodicLogs.Models.DB.Entries.DailyEntries.DailyEntry", b =>
                {
                    b.HasBaseType("Core.Entities.BIPeriodicLogs.Models.DB.BIPeriodicLog");

                    b.HasDiscriminator().HasValue("DailyEntry");
                });

            modelBuilder.Entity("Core.Entities.BIPeriodicLogs.Models.DB.Entries.MonthlyEntries.MonthlyEntry", b =>
                {
                    b.HasBaseType("Core.Entities.BIPeriodicLogs.Models.DB.BIPeriodicLog");

                    b.HasDiscriminator().HasValue("MonthlyEntry");
                });

            modelBuilder.Entity("Core.Entities.BIPeriodicLogs.Models.DB.Entries.WeeklyEntries.WeeklyEntry", b =>
                {
                    b.HasBaseType("Core.Entities.BIPeriodicLogs.Models.DB.BIPeriodicLog");

                    b.HasDiscriminator().HasValue("WeeklyEntry");
                });

            modelBuilder.Entity("Core.Entities.BIPeriodicLogs.Models.DB.RT.AnnualRTs.AnnualRT", b =>
                {
                    b.HasBaseType("Core.Entities.BIPeriodicLogs.Models.DB.BIPeriodicLog");

                    b.HasDiscriminator().HasValue("AnnualRT");
                });

            modelBuilder.Entity("Core.Entities.BIPeriodicLogs.Models.DB.RT.DailyRTs.DailyRT", b =>
                {
                    b.HasBaseType("Core.Entities.BIPeriodicLogs.Models.DB.BIPeriodicLog");

                    b.HasDiscriminator().HasValue("DailyRT");
                });

            modelBuilder.Entity("Core.Entities.BIPeriodicLogs.Models.DB.RT.MonthlyRTs.MonthlyRT", b =>
                {
                    b.HasBaseType("Core.Entities.BIPeriodicLogs.Models.DB.BIPeriodicLog");

                    b.HasDiscriminator().HasValue("MonthlyRT");
                });

            modelBuilder.Entity("Core.Entities.BIPeriodicLogs.Models.DB.RT.WeeklyRTs.WeeklyRT", b =>
                {
                    b.HasBaseType("Core.Entities.BIPeriodicLogs.Models.DB.BIPeriodicLog");

                    b.HasDiscriminator().HasValue("WeeklyRT");
                });

            modelBuilder.Entity("Core.Entities.Packets.Models.DB.AlarmLists.AlarmList", b =>
                {
                    b.HasBaseType("Core.Entities.Packets.Models.DB.Packet");

                    b.HasDiscriminator().HasValue("AlarmList");
                });

            modelBuilder.Entity("Core.Entities.Packets.Models.DB.Announcements.Announcement", b =>
                {
                    b.HasBaseType("Core.Entities.Packets.Models.DB.Packet");

                    b.Property<int>("AnodeType")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Announcement");
                });

            modelBuilder.Entity("Core.Entities.Packets.Models.DB.Detections.Detection", b =>
                {
                    b.HasBaseType("Core.Entities.Packets.Models.DB.Packet");

                    b.Property<int>("AnodeSize")
                        .HasColumnType("int");

                    b.Property<bool>("IsMismatched")
                        .HasColumnType("bit");

                    b.Property<int>("MeasuredType")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Detection");
                });

            modelBuilder.Entity("Core.Entities.Packets.Models.DB.Furnaces.InFurnaces.InFurnace", b =>
                {
                    b.HasBaseType("Core.Entities.Packets.Models.DB.Packet");

                    b.Property<int>("AnodePosition")
                        .HasColumnType("int");

                    b.Property<int>("BakedPosition")
                        .HasColumnType("int");

                    b.Property<int>("FTASuckPit")
                        .HasColumnType("int");

                    b.Property<int>("FTAinPIT")
                        .HasColumnType("int");

                    b.Property<int>("GreenPosition")
                        .HasColumnType("int");

                    b.Property<int>("OriginID")
                        .HasColumnType("int");

                    b.Property<int>("PITHeight")
                        .HasColumnType("int");

                    b.Property<int>("PITNumber")
                        .HasColumnType("int");

                    b.Property<int>("PITSectionNumber")
                        .HasColumnType("int");

                    b.Property<int>("PalletSide")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("TSLoad")
                        .HasColumnType("datetimeoffset");

                    b.HasDiscriminator().HasValue("InFurnace");
                });

            modelBuilder.Entity("Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces.OutFurnace", b =>
                {
                    b.HasBaseType("Core.Entities.Packets.Models.DB.Packet");

                    b.Property<int>("FTAPickUp")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("TSCentralConveyor")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("TSUnpackPIT")
                        .HasColumnType("datetimeoffset");

                    b.HasDiscriminator().HasValue("OutFurnace");
                });

            modelBuilder.Entity("Core.Entities.Packets.Models.DB.Shootings.Shooting", b =>
                {
                    b.HasBaseType("Core.Entities.Packets.Models.DB.Packet");

                    b.Property<int>("AnodeIDKey")
                        .HasColumnType("int");

                    b.Property<string>("GlobalStationStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LedStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProcedurePerformance")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ShootingTS")
                        .HasColumnType("datetimeoffset");

                    b.HasDiscriminator().HasValue("Shooting");
                });

            modelBuilder.Entity("Core.Entities.Packets.Models.DB.Announcements.S1S2Announcement.S1S2Announcement", b =>
                {
                    b.HasBaseType("Core.Entities.Packets.Models.DB.Announcements.Announcement");

                    b.Property<int>("SerialNumber")
                        .HasColumnType("int");

                    b.Property<int>("TrolleyNumber")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("S1S2Announcement");
                });

            modelBuilder.Entity("Core.Entities.Packets.Models.DB.Shootings.S3S4Shootings.S3S4Shooting", b =>
                {
                    b.HasBaseType("Core.Entities.Packets.Models.DB.Shootings.Shooting");

                    b.Property<bool>("IsDoubleAnode")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("S3S4Shooting");
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

            modelBuilder.Entity("Core.Entities.User.Models.DB.Acts.ActEntities.ActEntity", b =>
                {
                    b.HasOne("Core.Entities.User.Models.DB.Acts.Act", "Act")
                        .WithMany()
                        .HasForeignKey("ActID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Act");
                });

            modelBuilder.Entity("Core.Entities.User.Models.DB.Acts.ActEntities.ActEntityRoles.ActEntityRole", b =>
                {
                    b.HasOne("Core.Entities.User.Models.DB.Acts.ActEntities.ActEntity", "ActEntity")
                        .WithMany("ActEntityRoles")
                        .HasForeignKey("ActEntityID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActEntity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Core.Entities.User.Models.DB.Roles.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Core.Entities.User.Models.DB.Users.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Core.Entities.User.Models.DB.Users.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Core.Entities.User.Models.DB.Roles.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.User.Models.DB.Users.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Core.Entities.User.Models.DB.Users.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("Core.Entities.User.Models.DB.Acts.ActEntities.ActEntity", b =>
                {
                    b.Navigation("ActEntityRoles");
                });

            modelBuilder.Entity("Core.Entities.Packets.Models.DB.AlarmLists.AlarmList", b =>
                {
                    b.Navigation("AlarmCycles");
                });
#pragma warning restore 612, 618
        }
    }
}
