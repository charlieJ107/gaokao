using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gaokao.Data;
using Gaokao.Data.Models;
using System.ComponentModel.DataAnnotations;


namespace Gaokao.Pages.Report
{
    public class DetailsModel : PageModel
    {
        private readonly Gaokao.Data.ApplicationDbContext _context;

        public DetailsModel(Gaokao.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public class ListModel
        {
            [Display(Name = "学校")]
            public School school { get; set; }
            [Display(Name = "专业")]
            public Major major { get; set; }
            [Display(Name = "最高分省控线差")]
            public int MaxScoreDelta { get; set; }
            [Display(Name = "最低分省控线差")]
            public int MinScoreDelta { get; set; }
            [Display(Name = "平均分省控线差")]
            public int AveScoreDelta { get; set; }
            [Display(Name = "最高分排名")]
            public int MaxRank { get; set; }

            [Display(Name = "最低分排名")]
            public int MinRank { get; set; }
            [Display(Name = "平均分省控线排名")]
            public int AveRank { get; set; }
            IList<Enroll_Distribution> Enroll_Distributions { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="major"></param>
        /// <param name="provice"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        private async Task<ListModel> GetListModelAsync(Major major, string provice, int year)
        {
            var thisData = await major.Enroll_Data
                    .AsQueryable()
                    .FirstOrDefaultAsync(data =>
                    data.Provice == provice && data.Year == year);

            ListModel listModel = new ListModel
            {
                major=major,
                school=major.School,
                MaxRank=thisData.Max_Rank,
                MinRank=thisData.Min_Rank,
                AveRank=thisData.Ave_Rank,
                MaxScoreDelta=thisData.Max_Score_Delta,
                MinScoreDelta=thisData.Min_Score_Delta,
                AveScoreDelta=thisData.Ave_Score_Delta
            };


            return listModel;
        }

        private Major Major { get; set; }
        public ListModel ShowModel { get; set; }
        

        public async Task<IActionResult> OnGetAsync(int? id, string provice, int? year)
        {
            if (id == null || provice==null || year==null)
            {
                return NotFound();
            }


            Major = await _context.Major
                .Include(m => m.School)
                .Include(m=>m.Enroll_Data)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Major == null)
            {
                return NotFound();
            }

            ShowModel = await GetListModelAsync(Major, provice, year.Value);
            
            return Page();
        }
    }
}
