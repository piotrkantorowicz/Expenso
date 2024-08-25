﻿// <auto-generated />
using System;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Migrations
{
    [DbContext(typeof(BudgetSharingDbContext))]
    partial class BudgetSharingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("BudgetSharing")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Expenso.BudgetSharing.Domain.BudgetPermissionRequests.BudgetPermissionRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("BudgetId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ParticipantId")
                        .HasColumnType("uuid");

                    b.Property<int>("PermissionType")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.ToTable("BudgetPermissionRequests", "BudgetSharing");
                });

            modelBuilder.Entity("Expenso.BudgetSharing.Domain.BudgetPermissions.BudgetPermission", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("BudgetId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BudgetId")
                        .IsUnique();

                    b.ToTable("BudgetPermissions", "BudgetSharing");
                });

            modelBuilder.Entity("Expenso.BudgetSharing.Domain.BudgetPermissions.BudgetPermission", b =>
                {
                    b.OwnsMany("Expenso.BudgetSharing.Domain.BudgetPermissions.Permission", "Permissions", b1 =>
                        {
                            b1.Property<Guid>("BudgetPermissionId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("ParticipantId")
                                .HasColumnType("uuid");

                            b1.Property<int>("PermissionType")
                                .HasColumnType("integer");

                            b1.HasKey("BudgetPermissionId", "Id");

                            b1.ToTable("Permissions", "BudgetSharing");

                            b1.WithOwner()
                                .HasForeignKey("BudgetPermissionId");
                        });

                    b.OwnsOne("Expenso.Shared.Domain.Types.ValueObjects.Blocker", "Blocker", b1 =>
                        {
                            b1.Property<Guid>("BudgetPermissionId")
                                .HasColumnType("uuid");

                            b1.Property<DateTimeOffset?>("BlockDate")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<bool>("IsBlocked")
                                .HasColumnType("boolean");

                            b1.HasKey("BudgetPermissionId");

                            b1.ToTable("BudgetPermissions", "BudgetSharing");

                            b1.WithOwner()
                                .HasForeignKey("BudgetPermissionId");
                        });

                    b.Navigation("Blocker");

                    b.Navigation("Permissions");
                });
#pragma warning restore 612, 618
        }
    }
}
