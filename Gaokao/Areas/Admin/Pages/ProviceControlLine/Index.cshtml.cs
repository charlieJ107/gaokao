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
    public class IndexModel : PageModel
    {
        private readonly Gaokao.Data.ApplicationDbContext _context;

        public IndexModel(Gaokao.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ControlLine> ControlLine { get;set; }

        public async Task OnGetAsync()
        {
            ControlLine = await _context.ControlLine.ToListAsync();
        }
    }
}
