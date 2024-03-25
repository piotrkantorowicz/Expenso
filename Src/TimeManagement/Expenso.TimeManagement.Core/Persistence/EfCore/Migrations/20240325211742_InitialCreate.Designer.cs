﻿// <auto-generated />
using System;
using Expenso.TimeManagement.Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Migrations
{
    [DbContext(typeof(TimeManagementDbContext))]
    [Migration("20240325211742_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("TimeManagement")
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Expenso.TimeManagement.Core.Domain.Jobs.Model.JobEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("JobEntryStatusId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("JobEntryTypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("JobEntryStatusId");

                    b.HasIndex("JobEntryTypeId");

                    b.ToTable("JobEntries", "TimeManagement");
                });

            modelBuilder.Entity("Expenso.TimeManagement.Core.Domain.Jobs.Model.JobEntryStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.ToTable("JobEntryStatuses", "TimeManagement");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f156ddcf-5889-4d8e-8299-4d54971fe74e"),
                            Description = "The job entry is currently running.",
                            Name = "Running"
                        },
                        new
                        {
                            Id = new Guid("dc2678d1-3858-4740-aaa2-80cbfb4b69bc"),
                            Description = "The job entry has completed successfully.",
                            Name = "Completed"
                        },
                        new
                        {
                            Id = new Guid("214909c9-e0e2-4e93-a51c-cf1aae83a3ae"),
                            Description = "The job entry has failed.",
                            Name = "Failed"
                        },
                        new
                        {
                            Id = new Guid("b9532f23-22df-433c-8603-bb79068eeb40"),
                            Description = "The job entry is being retried.",
                            Name = "Retrying"
                        },
                        new
                        {
                            Id = new Guid("b465800e-dfeb-4596-9f13-77f15b17acee"),
                            Description = "The job entry has been cancelled.",
                            Name = "Cancelled"
                        });
                });

            modelBuilder.Entity("Expenso.TimeManagement.Core.Domain.Jobs.Model.JobEntryType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<int>("Interval")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.ToTable("JobEntryTypes", "TimeManagement");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0185f3ae-1a77-460a-8ade-978108d41289"),
                            Code = "BS-REQ-EXP",
                            Interval = 10,
                            Name = "Budget Sharing Requests Expiration"
                        });
                });

            modelBuilder.Entity("Expenso.TimeManagement.Core.Domain.Jobs.Model.JobEntry", b =>
                {
                    b.HasOne("Expenso.TimeManagement.Core.Domain.Jobs.Model.JobEntryStatus", "JobStatus")
                        .WithMany()
                        .HasForeignKey("JobEntryStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Expenso.TimeManagement.Core.Domain.Jobs.Model.JobEntryType", "JobEntryType")
                        .WithMany()
                        .HasForeignKey("JobEntryTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Expenso.TimeManagement.Core.Domain.Jobs.Model.JobEntryPeriod", "Periods", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid");

                            b1.Property<int>("Interval")
                                .HasColumnType("integer");

                            b1.Property<bool?>("IsCompleted")
                                .HasColumnType("boolean");

                            b1.Property<Guid>("JobEntryId")
                                .HasColumnType("uuid");

                            b1.Property<DateTimeOffset?>("LastRun")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<DateTimeOffset>("RunAt")
                                .HasColumnType("timestamp with time zone");

                            b1.HasKey("Id");

                            b1.HasIndex("JobEntryId");

                            b1.ToTable("JobEntryPeriods", "TimeManagement");

                            b1.WithOwner()
                                .HasForeignKey("JobEntryId");
                        });

                    b.OwnsMany("Expenso.TimeManagement.Core.Domain.Jobs.Model.JobEntryTrigger", "Triggers", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid");

                            b1.Property<string>("EventData")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("EventType")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("character varying(150)");

                            b1.Property<Guid>("JobEntryId")
                                .HasColumnType("uuid");

                            b1.HasKey("Id");

                            b1.HasIndex("JobEntryId");

                            b1.ToTable("JobEntryTriggers", "TimeManagement");

                            b1.WithOwner()
                                .HasForeignKey("JobEntryId");
                        });

                    b.Navigation("JobEntryType");

                    b.Navigation("JobStatus");

                    b.Navigation("Periods");

                    b.Navigation("Triggers");
                });
#pragma warning restore 612, 618
        }
    }
}
