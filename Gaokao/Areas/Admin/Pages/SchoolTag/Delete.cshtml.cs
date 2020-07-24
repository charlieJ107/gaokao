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

namespace Gaokao.Pages.Admin.SchoolTag
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly Gaokao.Data.ApplicationDbContext _context;

        public DeleteModel(Gaokao.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Tag_of_School Tag_of_School { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Tag_of_School = await _context.Tag_of_School.FirstOrDefaultAsync(m => m.ID == id);

            if (Tag_of_School == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Tag_of_School = await _context.Tag_of_School.FindAsync(id);

            if (Tag_of_School != null)
            {
                _context.Tag_of_School.Remove(Tag_of_School);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
