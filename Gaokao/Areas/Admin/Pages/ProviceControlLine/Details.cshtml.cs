using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gaokao.Data;
using Gaokao.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace Gaokao.Areas.Admin.Pages.ProviceControlLine
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly Gaokao.Data.ApplicationDbContext _context;

        public DetailsModel(Gaokao.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public ControlLine ControlLine { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ControlLine = await _context.ControlLine.FirstOrDefaultAsync(m => m.ID == id);

            if (ControlLine == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
