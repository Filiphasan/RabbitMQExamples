﻿using Microsoft.EntityFrameworkCore;
using WatermarkMQ.Models;

namespace WatermarkMQ.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
    }
}
