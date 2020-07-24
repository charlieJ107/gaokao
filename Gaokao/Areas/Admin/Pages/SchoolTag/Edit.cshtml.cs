using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gaokao.Data;
using Gaokao.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace Gaokao.Pages.Admin.SchoolTag
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly Gaokao.Data.ApplicationDbContext _context;

        public EditModel(Gaokao.Data.ApplicationDbContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Tag_of_School).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tag_of_SchoolExists(Tag_of_School.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool Tag_of_SchoolExists(int id)
        {
            return _context.Tag_of_School.Any(e => e.ID == id);
        }
    }
}
