using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Gaokao.Areas.Identity.Data;
using Gaokao.Data;
using Gaokao.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Gaokao.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

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
        

        /// <summary>
        /// 页面的输入模块
        /// </summary>
        public class InputModel
        {
            [Phone]
            [Display(Name = "电话号码")]
            public string PhoneNumber { get; set; }

            //////////自定义用户标识数据//////////
            [Required]
            [Display(Name ="分数")]
            [Range(1, 750, ErrorMessage = "你真考了这个分数?\n(如果是真的, 联系一下管理员吧!)")]
            public int OwnScore { get; set; }
            [Required]
            [Display(Name ="省份")]
            public string Provice { get; set; }
            [Required]
            [Display(Name ="年份")]
            public int Year { get; set; }
            
            [Display(Name = "排名")]
            public int OwnRank { get; set; }
            ////////////自定义标识结束///////////
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,

                ////////自定义用户数据////////
                OwnScore=user.OwnScore,
                Year=user.Year,
                Provice=user.Provice,
                OwnRank=user.OwnRank
                //////自定义用户数据结束///////
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"无法加载用户id为 '{_userManager.GetUserId(User)}'的用户. 请检查登录状态. ");
            }


            /////////////省份和年份查询构造下拉列表/////////////
            IQueryable<string> proviceQuery = from m in _context.Score_Rank_Table
                                              orderby m.Provice
                                              select m.Provice;
            IQueryable<int> yearQuery = from table in _context.Score_Rank_Table
                                        orderby table.Year
                                        select table.Year;

            //////////////构造下拉列表结束/////////////////////

            await LoadAsync(user);
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
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            /////自定义用户数据//////

            if(Input.Provice!=user.Provice)
            {
                user.Provice = Input.Provice;
            }
            if(Input.Year!=user.Year)
            {
                user.Year = Input.Year;
            }
            if(Input.OwnScore!=user.OwnScore)
            {
                user.OwnScore = Input.OwnScore;
                int userRank = await CaculateRank(user.OwnScore, user.Provice, user.Year);
                user.OwnRank = userRank;
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        /// <summary>
        /// 根据分数和年份省份来确定这个人的排名
        /// </summary>
        /// <param name="score">分数</param>
        /// <param name="provice">省份</param>
        /// <param name="year">年份</param>
        /// <returns>这个人在当年的排名</returns>
        private async Task<int> CaculateRank(int score, string provice, int year)
        {
            Score_Rank_Table Score_Rank_Table = await _context.Score_Rank_Table
                .Where<Score_Rank_Table>(i => i.Provice == provice)
                .Where(i => i.Year == year)
                .FirstOrDefaultAsync();
            var theRank = Score_Rank_Table
                .Score_Rank_Items_National_Old
                .FirstOrDefault(b => b.Score == score).Rank;
            return theRank;
        }
    }
}
