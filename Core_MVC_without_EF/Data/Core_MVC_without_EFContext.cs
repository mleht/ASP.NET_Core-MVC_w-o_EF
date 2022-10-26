using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core_MVC_without_EF.Models;

namespace Core_MVC_without_EF.Data
{
    public class Core_MVC_without_EFContext : DbContext
    {
        public Core_MVC_without_EFContext (DbContextOptions<Core_MVC_without_EFContext> options)
            : base(options)
        {
        }

        public DbSet<Core_MVC_without_EF.Models.MovieViewModel> MovieViewModel { get; set; }
    }
}
