using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRpStudent.Models
{
    public partial class Student
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [StringLength(255)]
        public string 序号 { get; set; }
        [StringLength(255)]
        public string 入学年份 { get; set; }
        [StringLength(255)]
        public string 班级 { get; set; }
        [StringLength(255)]
        public string 姓名 { get; set; }
        [StringLength(255)]
        public string 性别 { get; set; }
        [StringLength(255)]
        public string 学籍号 { get; set; }
        [StringLength(255)]
        public string 备注 { get; set; }
    }
}
