using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gaokao.Data;
using Gaokao.Data.Models;

namespace Gaokao.Pages.Info.Score_Rank._2019
{
    public class NationalOldModel : PageModel
    {
        private readonly Gaokao.Data.ApplicationDbContext _context;

        public NationalOldModel(Gaokao.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 用于加载想要的List的表
        /// </summary>
        /// <param name="provice">省份的中文字符串</param>
        /// <param name="year">年份的整数</param>
        /// <param name="searchString">搜索字符串, 如果为空字符串就返回整个列表</param>
        /// <returns></returns>
        private async Task<IList<Score_Rank_Items_National_Old>> loadListAsync(string provice, int? year, string searchString, string type)
        {
            Score_Rank_Table theTable = await _context.Score_Rank_Table
                .Where<Score_Rank_Table>(t => t.Provice == provice && t.Year == year)
                .Include(t => t.Score_Rank_Items_National_Old)
                .FirstOrDefaultAsync();
            var ScoreItem = from i in theTable.Score_Rank_Items_National_Old.ToList()
                            where i.type==type
                            select i;
            if (!string.IsNullOrEmpty(SearchString))
            {
                int searchScore = Convert.ToInt32(SearchString);//将字符串转为int用于比较和查找
                ScoreItem = ScoreItem.Where(s => s.Score.Equals(searchScore));
            }
            return theTable.Score_Rank_Items_National_Old.ToList();
        }

        /// <summary>
        /// 用于存储一分一档表栏目的List
        /// </summary>
        public IList<Score_Rank_Items_National_Old> Score_Rank_Items_List { get; set; }

        /// <summary>
        /// 添加搜索-搜索字符串
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        /// <summary>
        /// http get时的主方法, 也是页面的主方法
        /// </summary>
        /// <param name="provice">省份</param>
        /// <param name="year">年份</param>
        /// <param name="type">考生类别, 理工类还是文史类</param>
        /// <returns>页面的IActionResult</returns>
        public async Task<IActionResult> OnGetAsync(string provice, int? year, string type)
        {
            if (provice == "" || year == null)
            {
                return NotFound();
            }
            else
            {
                //到这儿则传入了省份和年份参数
                //但省份和年份参数可能是错的
                Score_Rank_Items_List = await loadListAsync(provice, year, SearchString, type);
                if (Score_Rank_Items_List == null)
                {
                    return NotFound();
                }
                else
                {
                    //构造
                    ViewData["Title"] = (year.ToString()) + provice + "高考一分一档表";
                    ViewData["Year"] = year.ToString();
                    ViewData["Provice"] = provice;
                    ViewData["Type"] = type;
                    return Page();
                }
            }

        }
    }
}
