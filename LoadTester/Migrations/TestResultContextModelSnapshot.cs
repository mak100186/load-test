// <auto-generated />
using System;
using LoadTester.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LoadTester.Migrations
{
    [DbContext(typeof(TestResultContext))]
    partial class TestResultContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LoadTester.Repository.Entities.TestResults", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AverageMemoryFootprintPerCall")
                        .HasColumnType("bigint");

                    b.Property<long>("AverageMillisecondsPerCall")
                        .HasColumnType("bigint");

                    b.Property<long>("ElapsedMilliseconds")
                        .HasColumnType("bigint");

                    b.Property<long>("MaxMemoryFootprint")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("PerformedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ResultString")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TotalCalls")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("TestResults");
                });
#pragma warning restore 612, 618
        }
    }
}
