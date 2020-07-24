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
    public class MajorDetailModel : PageModel
    {
        private readonly Gaokao.Data.ApplicationDbContext _context;

        public MajorDetailModel(Gaokao.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Major Major { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Major = await _context.Major
                .Include(m => m.School).FirstOrDefaultAsync(m => m.ID == id);

            if (Major == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
