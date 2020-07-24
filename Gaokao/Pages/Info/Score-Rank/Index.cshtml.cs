using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gaokao.Data;
using Gaokao.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gaokao.Pages.Info.Score_Rank
{
    public class IndexModel : PageModel
    {
        private readonly Gaokao.Data.ApplicationDbContext _context;

        public IndexModel(Gaokao.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 所有的Score_Rank_Table的表, 用于渲染页面
        /// </summary>
        public IList<Score_Rank_Table> Score_Rank_Table { get; set; }

        //////////////////////////////////省份查询需要的东西////////////////////
        
        /// <summary>
        /// 用来构建省份查询的下拉框
        /// </summary>
        public SelectList ProvicesList { get; set; }
        
        /// <summary>
        /// 用来构建省份查询条件
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string ProviceString { get; set; }

        ///////////////////////////////////省份查询的东西结束///////////////////
        ///////////////////////////////////年份查询需要的东西///////////////////
        
        /// <summary>
        /// 用来构建年份查询的下拉数据
        /// </summary>
        public SelectList YearList { get; set; }

        /// <summary>
        /// 用来构造年份查询条件
        /// </summary>
        [BindProperty(SupportsGet =true)]
        public string YearString { get; set; }
        
        //////////////////////////////////年份查询需要的东西结束/////////////////

        public async Task OnGetAsync()
        {

            //////按省份查询
            IQueryable<string> proviceQuery = from m in _context.Score_Rank_Table
                                              orderby m.Provice
                                              select m.Provice;
            ProvicesList = new SelectList(await proviceQuery.Distinct().ToListAsync());

            ///按年份查询
            ///
            IQueryable<int> yearQuery = from table in _context.Score_Rank_Table
                                        orderby table.Year
                                        select table.Year;
            YearList = new SelectList(await yearQuery.Distinct().ToListAsync());
            var tables = from table in _context.Score_Rank_Table
                         select table;
            if (!string.IsNullOrEmpty(ProviceString))
            {
                tables = tables.Where(x => x.Provice == ProviceString);
            }
            if(!string.IsNullOrEmpty(YearString))
            {
                int yearNumber = Convert.ToInt32(YearString);
                tables = tables.Where(x => x.Year == yearNumber);
            }

            Score_Rank_Table = await tables.ToListAsync();
        }
    }
}
