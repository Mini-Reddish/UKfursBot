﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UKFursBot.Context;

#nullable disable

namespace UKFursBot.Migrations
{
    [DbContext(typeof(UKFursBotDbContext))]
    [Migration("20230716145206_added error logging channel")]
    partial class addederrorloggingchannel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("UKFursBot.Entities.AnnouncementMessage", b =>
                {
                    b.Property<long>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("MessageId"));

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MessagePurpose")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("MessageId");

                    b.ToTable("AnnouncementMessages");
                });

            modelBuilder.Entity("UKFursBot.Entities.BanOnJoin", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("ModID")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("UserID")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.ToTable("BansOnJoin");
                });

            modelBuilder.Entity("UKFursBot.Entities.BotConfiguration", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("AnnouncementChannelId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("ErrorLoggingChannelId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.ToTable("BotConfigurations");
                });

            modelBuilder.Entity("UKFursBot.Entities.ErrorLogging", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal?>("ChannelId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<decimal>("ServerId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.ToTable("ErrorLogging");
                });

            modelBuilder.Entity("UKFursBot.Entities.ModMail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal?>("ChannelId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<decimal?>("ModRoleId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("ServerId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.ToTable("ModMails");
                });

            modelBuilder.Entity("UKFursBot.Entities.UserNote", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Forgiven")
                        .HasColumnType("boolean");

                    b.Property<decimal>("ForgivenBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("ModeratorId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("UserId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.ToTable("UserNotes");
                });

            modelBuilder.Entity("UKFursBot.Entities.Warning", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Forgiven")
                        .HasColumnType("boolean");

                    b.Property<decimal>("ForgivenBy")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("ModeratorId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("UserId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.ToTable("Warnings");
                });
#pragma warning restore 612, 618
        }
    }
}
