using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gaokao.Data.Models
{
    public class Major
    {
        public int ID { get; set; }
        public School School { get; set; }
        public int SchoolId { get; set; }
        public string Type { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public string Level_Result { get; set; }
        public ICollection<Enroll_Overview> Enroll_Data { get; set; }
        public ICollection<Enroll_Distribution> Enroll_Data_Detail { get; set; }
    }

    public class Enroll_Overview
    {
        public string ID { get; set; }
        public int MajorId { get; set; }
        public int Year { get; set; }
        public string Provice { get; set; }
        public int Max_Score { get; set; }
        public int Min_Score { get; set; }
        public int Ave_Score { get; set; }
        public int Max_Rank { get; set; }
        public int Min_Rank { get; set; }
        public int Ave_Rank { get; set; }
        public int Max_Score_Delta { get; set; }
        public int Min_Score_Delta { get; set; }
        public int Ave_Score_Delta { get; set; }
        public Major Major { get; set; }

    }

    public class Enroll_Distribution
    {
        public string ID { get; set; }
        public int Year { get; set; }
        public string Provice { get; set; }
        public int MajorId { get; set; }
        public Major Major { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
        public int People { get; set; }
    }
}
