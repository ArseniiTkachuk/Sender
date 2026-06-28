using Microsoft.EntityFrameworkCore;
using Sender.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sender.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
