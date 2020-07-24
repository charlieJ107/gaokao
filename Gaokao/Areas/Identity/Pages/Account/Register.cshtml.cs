using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Gaokao.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Gaokao.Data;

namespace Gaokao.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = " {0} 应至少 {2} 长并且不多于 {1} 个字符.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "密码")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "密码确认")]
            [Compare("Password", ErrorMessage = "两次输入的密码不匹配")]
            public string ConfirmPassword { get; set; }


            //[Display(Name = "分数")]
            //[Range(1, 750, ErrorMessage = "你真考了这个分数?\n(如果是真的, 联系一下管理员吧!)")]
            //public int Score { get; set; }

            //[Display(Name = "省份")]
            //public string Provice { get; set; }

            //[Display(Name = "高考年份")]
            //public int Year { get; set; }

        }

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


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            /////////////省份和年份查询构造下拉列表/////////////
            IQueryable<string> proviceQuery = from m in _context.Score_Rank_Table
                                              orderby m.Provice
                                              select m.Provice;
            IQueryable<int> yearQuery = from table in _context.Score_Rank_Table
                                        orderby table.Year
                                        select table.Year;

            //////////////构造下拉列表结束/////////////////////

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                };
                //if(Input.Score!=null)
                //{
                //    user.OwnScore = Input.Score;

                //}
                //if 
                //user.Provice = Input.Provice;
                //    user.Year = Input.Year;

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "[环己来-高考] 请确认您的账户邮箱",
                        $"请 <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>点击这里</a>以确认您的邮箱账户.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
