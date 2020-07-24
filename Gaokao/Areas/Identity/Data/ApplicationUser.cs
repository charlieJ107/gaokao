using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Gaokao.Areas.Identity.Data
{
    public class ApplicationUser: IdentityUser
    {
        [PersonalData]
        public int OwnScore { get; set; }
        [PersonalData]
        public int OwnRank { get; set; }
        [PersonalData]
        public string Provice { get; set; }
        [PersonalData]
        public int Year { get; set; }
    }
}
