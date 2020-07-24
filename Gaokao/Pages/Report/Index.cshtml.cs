using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gaokao.Data;
using Gaokao.Data.Models;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;

namespace Gaokao.Pages.Report
{
    public class IndexModel : PageModel
    {
        private readonly Gaokao.Data.ApplicationDbContext _context;

        public IndexModel(Gaokao.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public class ListModel
        {
            [Display(Name ="学校")]
            public School school { get; set; }
            [Display(Name = "专业")]
            public Major major { get; set; }
            [Display(Name ="最高分省控线差")]
            public int MaxScoreDelta { get; set; }
            [Display(Name ="最低分省控线差")]
            public int MinScoreDelta { get; set; }
            [Display(Name ="平均分省控线差")]
            public int AveScoreDelta { get; set; }
            [Display(Name ="最高分排名")]
            public int MaxRank { get; set; }

            [Display(Name ="最低分排名")]
            public int MinRank { get; set; }
            [Display(Name ="平均分省控线排名")]
            public int AveRank { get; set; }

            public int Year { get; set; }
            public string Provice { get; set; }
        }

        /// <summary>
        /// 根据分数和年份省份来确定这个人的排名
        /// </summary>
        /// <param name="score">分数</param>
        /// <param name="provice">省份</param>
        /// <param name="year">年份</param>
        /// <returns>这个人在当年的排名</returns>
        private int CaculateRank(int score, string provice, int year)
        {
            Score_Rank_Table Score_Rank_Table = _context.Score_Rank_Table
                .Where<Score_Rank_Table>(i => i.Provice == provice)
                .Where(i => i.Year == year)
                .FirstOrDefault();
            var theRank = Score_Rank_Table
                .Score_Rank_Items_National_Old
                .FirstOrDefault(b => b.Score == score).Rank;
            return theRank;
        }

        /// <summary>
        /// 用于在读写数据库时判断这个Major是否符合要求的函数
        /// </summary>
        /// <param name="majorItem">用于判断的Major</param>
        /// <param name="provice">省份</param>
        /// <param name="year">年份</param>
        /// <param name="score">分数</param>
        /// <param name="scoreRange">分数允许波动的区间</param>
        /// <param name="rankRange">排名允许波动的区间</param>
        /// <returns>
        /// 如果这个专业的录取数据中分数和排名分别在波动区间里则返回True, 否则返回False
        /// </returns>
        private bool SelectMajor(Major majorItem, string provice, int year, int score, int scoreRange, int rankRange)
        {
            ICollection<Enroll_Overview> Enroll_collection = majorItem.Enroll_Data;
            var yes_score_data = from data in Enroll_collection
                                 //今年的考生要查去年的数据, 所以year-1
                                 where data.Provice==provice && data.Year == year-1
                                 where data.Min_Score > (score - scoreRange) && data.Max_Score < score + scoreRange
                                 select data;
            int userRank = CaculateRank(score, provice, year);
            var yes_rank_data = from data in Enroll_collection
                                where data.Provice==provice
                                where CaculateRank(data.Min_Score, provice, data.Year) > (userRank - rankRange) || CaculateRank(data.Max_Score, provice, data.Year) < (rankRange + userRank)
                                select data;
            return yes_score_data.Any(m => m.MajorId == majorItem.ID) || yes_rank_data.Any(m => m.MajorId == majorItem.ID);
        }

        /// <summary>
        /// 根据专业数据获取展示的对象
        /// </summary>
        /// <param name="major">专业数据对象</param>
        /// <param name="provice">考生查询的省份</param>
        /// <param name="year">考生查询的年份</param>
        /// <returns>可用于展示的ListModel对象</returns>
        private async Task<ListModel> GetShowItem(Major major, string provice, int year)
        {
            var thisData = await major.Enroll_Data
                                .AsQueryable()
                                .FirstOrDefaultAsync(data =>
                                data.Provice == provice && data.Year == year);
            
            ListModel item = new ListModel
            {
                major = major,
                school = major.School,
                MaxScoreDelta=thisData.Max_Score_Delta,
                MinScoreDelta=thisData.Min_Score_Delta,
                AveScoreDelta=thisData.Ave_Score_Delta,
                MaxRank=thisData.Max_Rank,
                MinRank=thisData.Min_Rank,
                AveRank=thisData.Ave_Rank,
            };
            return item;
        }

        public bool IsLoadReady { get; set; }
        public IList<ListModel> ShowList { get; set; }


        public async Task OnGetAsync(string provice, int year, int score, int scoreRange, int rankRange)
        {

            Console.WriteLine(Url);
            IsLoadReady = false;
            IList<Major> Major = await _context.Major
                .Where(m => 
                SelectMajor(m, provice, year, score, scoreRange, rankRange))
                .Include(m=>m.School)
                .Include(m=>m.Enroll_Data)
                .ToListAsync();
            foreach (Major m in Major)
            {
                ListModel theShowItem =await GetShowItem(m, provice, year);
                ShowList.Add(theShowItem);
            }
            IsLoadReady = true;
        }
    }
}
