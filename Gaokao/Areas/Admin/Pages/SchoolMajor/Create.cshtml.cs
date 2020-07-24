using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Gaokao.Data;
using Gaokao.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace Gaokao.Areas.Admin.Pages.SchoolMajor
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly Gaokao.Data.ApplicationDbContext _context;

        public CreateModel(Gaokao.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["SchoolId"] = new SelectList(_context.School, "ID", "ID");
            return Page();
        }

        [BindProperty]
        public Major Major { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Major.Add(Major);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
