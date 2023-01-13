using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LoadTester.Repository.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoadTester.Repository;
public class TestResultContext : DbContext
{
    public DbSet<TestResults> TestResults => this.Set<TestResults>();

    public TestResultContext(DbContextOptions<TestResultContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Call base method to create everything
        base.OnModelCreating(modelBuilder);

        // Afterwards, add more configuration to how model is created
        modelBuilder.ApplyConfiguration(new TestResultsConfiguration());
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        optionsBuilder.EnableSensitiveDataLogging();


        base.OnConfiguring(optionsBuilder);
    }
}

public class TestResultsConfiguration : IEntityTypeConfiguration<TestResults>
{
    public void Configure(EntityTypeBuilder<TestResults> entityBuilder)
    {
        entityBuilder.HasIndex(c => c.Id);
    }
}
