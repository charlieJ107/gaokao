using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gaokao.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Gaokao.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Gaokao.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace Gaokao.Pages.Form
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public IndexModel(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// 定义输入模型类
        /// </summary>
        public class InputModel
        {
            [Required]
            [Display(Name = "分数")]
            [Range(1, 750, ErrorMessage = "你真考了这个分数?\n(如果是真的, 联系一下管理员吧!)")]
            public int Score { get; set; }
            [Required]
            [Display(Name = "省份")]
            public string Provice { get; set; }
            [Required]
            [Display(Name = "年份")]
            public int Year { get; set; }
            [Required]
            [Display(Name ="分数波动范围")]
            public int ScoreRange { get; set; }
            [Required]
            [Display(Name ="排名波动范围")]
            public int RankRange { get; set; }

        }

        /// <summary>
        /// 声明输入模型
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }


        /////////////用于定义下拉框的一些必要变量/////////

        /// <summary>
        /// 用来构建省份查询的下拉框
        /// </summary>
        public SelectList ProvicesList { get; set; }

        /// <summary>
        /// 用来构建年份查询的下拉数据
        /// </summary>
        public SelectList YearList { get; set; }

        //////////////////下拉框变量结束/////////////////


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"无法加载用户id为'{_userManager.GetUserId(User)}'的用户, 请检查登录状态.");
            }


            /////////////省份和年份查询构造下拉列表/////////////
            IQueryable<string> proviceQuery = from m in _context.Score_Rank_Table
                                              orderby m.Provice
                                              select m.Provice;
            IQueryable<int> yearQuery = from table in _context.Score_Rank_Table
                                        orderby table.Year
                                        select table.Year;

            //////////////构造下拉列表结束/////////////////////


            Input = new InputModel
            {
                ////////自定义用户数据////////
                Score = user.OwnScore,
                Year = user.Year,
                Provice = user.Provice,
                //////自定义用户数据结束///////
            };

            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"无法加载用户id为'{_userManager.GetUserId(User)}'的用户, 请检查登录状态.");
            }

            if (!ModelState.IsValid)
            {
                Input = new InputModel
                {
                    ////////自定义用户数据////////
                    Score = user.OwnScore,
                    Year = user.Year,
                    Provice = user.Provice,
                    //////自定义用户数据结束///////
                };
                return Page();
            }
            else
            {

                return RedirectToPage("/Report/Index", new { 
                    year= Input.Year, 
                    provice=Input.Provice, 
                    score=Input.Score, 
                    scoreRange=Input.ScoreRange, 
                    rankRange=Input.RankRange});
            }


        }
    }
}