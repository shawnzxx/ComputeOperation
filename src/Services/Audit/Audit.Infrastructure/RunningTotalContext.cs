using Audit.Domain.Models;
using Audit.Infrastructure.EntityConfigurations;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Infrastructure
{
    public class RunningTotalContext : DbContext
    {
        public  DbSet<RunningTotal> RunningTotal { get; set; }

        //we add RunningTotalContext to the ASP.net Core IoC container with options, so that we can use it in Startup.cs
        public RunningTotalContext(DbContextOptions<RunningTotalContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RunningToolEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
