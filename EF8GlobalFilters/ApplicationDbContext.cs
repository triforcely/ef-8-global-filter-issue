﻿using EF8GlobalFilters.Models;
using Microsoft.EntityFrameworkCore;

namespace EF8GlobalFilters;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasQueryFilter(p => p.DeletedAt != null);
    }
}