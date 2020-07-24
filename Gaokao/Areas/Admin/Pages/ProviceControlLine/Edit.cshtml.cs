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

namespace Gaokao.Areas.Admin.Pages.ProviceControlLine
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

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ControlLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ControlLineExists(ControlLine.ID))
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

        private bool ControlLineExists(int id)
        {
            return _context.ControlLine.Any(e => e.ID == id);
        }
    }
}
