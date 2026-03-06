using _2FAImplement.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace _2FAImplement.Data
{
    public class UserDbContext: DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)  : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
