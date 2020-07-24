using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections;



namespace Gaokao.Data.Models
{
    
    public class Score_Rank_Table
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// 
        [Key]
        public int Score_Rank_Table_ID { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        /// 
        [Required]
        [Display(Name ="年份")]
        public int Year { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        /// 
        [Required]
        [Display(Name ="省份")]
        [StringLength(20)]
        public string Provice { get; set; }
        [Required]
        [Display(Name ="类别")]
        [StringLength(50)]
        public string type { get; set; }
        /// <summary>
        /// 导航属性: 表内容ID的集合-全国卷(旧高考) 一分一档表 版本
        /// </summary>
        public ICollection<Score_Rank_Items_National_Old> Score_Rank_Items_National_Old { get; set; }
    }

    /// <summary>
    /// 全国卷(旧高考) 一分一档表items数据模型
    /// 
    /// 创建新的表items模型时, 应当在表items模型中和表模型中均添加导航属性, 并且在表Items模型中
    /// 设置外键
    /// </summary>
    /// 
    [Table("Score_Rank_Items_National_Old")]
    public class Score_Rank_Items_National_Old
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// 
        [Key]
        public int Score_Rank_Items_National_Old_ID { get; set; }
        /// <summary>
        /// 外键, 来自表ID
        /// </summary>
        ///
        [ForeignKey("Score_Rank_Table_ID")]
        public int Score_Rank_Table_ID { get; set; }
        /// <summary>
        /// 考生类别, 理科还是文科
        /// </summary>
        /// 
        [Display(Name = "考生类别")]
        [StringLength(50)]
        public string type { get; set; }
        /// <summary>
        /// 一分一档表-总分
        /// </summary>
        /// 
        [Display(Name = "总分")]
        public int Score { get; set; }
        /// <summary>
        /// 一分一档表-该分段人数
        /// </summary>
        /// 
        [Display(Name = "人数")]
        public int People { get; set; }
        /// <summary>
        /// 一分一档表-累计人数
        /// </summary>
        /// 
        [Display(Name = "累计人数")]
        public int TotalPeople { get; set; }
        /// <summary>
        /// 一分一档表-排名
        /// </summary>
        /// 
        [Display(Name = "排名")]
        public int Rank { get; set; }

        /// <summary>
        /// 导航属性, 导航至表
        /// </summary>
        public Score_Rank_Table Score_Rank_Table { get; set; }
    }

}
