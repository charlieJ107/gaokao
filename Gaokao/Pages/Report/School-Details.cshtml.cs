using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gaokao.Data;
using Gaokao.Data.Models;

namespace Gaokao.Pages.Report
{
    public class SchoolDetailsModel : PageModel
    {
        private readonly Gaokao.Data.ApplicationDbContext _context;

        public SchoolDetailsModel(Gaokao.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public School School { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            School = await _context.School.FirstOrDefaultAsync(m => m.ID == id);

            if (School == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
