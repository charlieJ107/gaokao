using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gaokao.Data.Models;
using Gaokao.Areas.Identity.Data;

namespace Gaokao.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Gaokao.Data.Models.Score_Rank_Items_National_Old> Score_Rank_Items_National_Old { get; set; }
        public DbSet<Gaokao.Data.Models.Score_Rank_Table> Score_Rank_Table { get; set; }
        public DbSet<Gaokao.Data.Models.Tag_of_School> Tag_of_School { get; set; }
        public DbSet<Gaokao.Data.Models.Major> Major { get; set; }
        public DbSet<Gaokao.Data.Models.School> School { get; set; }
        public DbSet<Gaokao.Data.Models.ControlLine> ControlLine { get; set; }
    }
}
