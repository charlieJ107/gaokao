using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gaokao.Data;
using Gaokao.Data.Models;

namespace Gaokao.Areas.Admin.Pages.Schools
{
    public class IndexModel : PageModel
    {
        private readonly Gaokao.Data.ApplicationDbContext _context;

        public IndexModel(Gaokao.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<School> School { get;set; }

        public async Task OnGetAsync()
        {
            School = await _context.School.ToListAsync();
        }
    }
}
