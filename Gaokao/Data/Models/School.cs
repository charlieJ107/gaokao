using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Gaokao.Data.Models
{
    public class School
    {
        public int ID { get; set; }
        [Display(Name="录取批次")]
        public string Name { get; set; }
        [Display(Name="所在省份")]
        public string Provice { get; set; }
        [Display(Name="所在城市")]
        public string City { get; set; }
        [Display(Name = "学科类型")]
        public string MajorType { get; set; }
        
        public string Level { get; set; }
        [Display(Name = "录取批次")]
        public string Batch { get; set; }
        [Display(Name = "一流大学")]
        public bool IsTopUniversity { get; set; }
        [Display(Name = "一流学科")]
        public bool IsTopMajor { get; set; }
        public ICollection<Major> Majors { get; set; }
        public ICollection<SchoolTag> SchoolTag { get; set; }
    }
    public class SchoolTag
    {
        public int ID { get; set; }
        public int SchoolId { get; set; }
        public int TagId { get; set; }
        public School School { get; set; }
        public Tag_of_School Tag { get; set; }
    }
    public class Tag_of_School
    {
        public int ID { get; set; }
        public string Tag_Name { get; set; }
        public ICollection<SchoolTag> SchoolTags { get; set; }
    }
}
