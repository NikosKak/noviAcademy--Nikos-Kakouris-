using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorldRank.Infrastructure
{
    public class WorldRankDbContextContextFactory : IDesignTimeDbContextFactory<WorldRankDbContext>
    {
        public WorldRankDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WorldRankDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress;Database=NoviAcademy;User=nikos;Password=12345;Integrated Security=true;TrustServerCertificate=true");

            return new WorldRankDbContext(optionsBuilder.Options);
        }
    }
}
