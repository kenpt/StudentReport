using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyRpStudent.Data;
using MyRpStudent.Models;
using X.PagedList;


namespace MyRpStudent
{
    public class IndexModel : PageModel
    {
        //注入DBcontext服务StudentReportContext（此服务已在Startup类IoC容器中注册）
        private readonly MyRpStudent.Data.StudentReportContext _context;

        public IndexModel(MyRpStudent.Data.StudentReportContext context)
        {
            _context = context;
        }

        public IPagedList<Student> Student { get;set; }

        public int NumberOfStudents { get; set; }
        public string CurrentFilter { get; set; }
        //public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public string EnrollmentYear { get; set; }
        public SelectList EnrollmentYearList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string StudentClass { get; set; }
        public SelectList StudentClassList { get; set; }

        public async Task OnGetAsync(string currentFilter,
            string searchString, int? pg)
        {

            IQueryable<Student> studentIQ = from st in _context.Student
                                            //where st.入学年份 == "2018" && st.班级 == "1"
                                       select st;
            //入学年份即年级
            IQueryable<string> enrollmentYearList = from s in _context.Student
                                                    //orderby s.入学年份 descending
                                                    select s.入学年份;
            //班级  --SearchString=李
            IQueryable<string> studentClassList = from c in _context.Student
                                                  //orderby c.班级 ascending
                                                  select c.班级;
            if (!string.IsNullOrEmpty(searchString))
            {
                pg ??= 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if(!string.IsNullOrEmpty(searchString))
            {
                studentIQ = studentIQ.Where(s => 
                    s.姓名.Contains(searchString)).OrderBy(s =>s.姓名);
            }
            if (!string.IsNullOrEmpty(EnrollmentYear))
            {
                studentIQ = studentIQ.Where(s => s.入学年份 == EnrollmentYear);
            }
            if (!string.IsNullOrEmpty(StudentClass))
            {
                studentIQ = studentIQ.Where(s => s.班级 == StudentClass);
            }
            CurrentFilter = searchString;   //用属性CurrentFilter保存当前搜索字串

            int pageNumber = pg ?? 1;
            EnrollmentYearList = new SelectList(await 
                enrollmentYearList.Distinct().OrderBy(x =>x).ToListAsync());
            StudentClassList = new SelectList(await 
                studentClassList.Distinct().OrderBy(x => Convert.ToInt32(x)).ToListAsync());

            //不要显示全部数据，没有查询时最多显示50条。
            //没有查询即查询关键字为空
            //记录数
            NumberOfStudents = studentIQ.Count();
            if ((searchString ?? EnrollmentYear ?? StudentClass) == null)
            {
                int minId = studentIQ.Min(x => x.Id);
                //int maxId = studentIQ.Max(x => x.Id);
                Student = await studentIQ.Where(x => x.Id < 50).ToPagedListAsync(pageNumber, 10);
            }
            else
            {
                Student = await studentIQ.ToPagedListAsync(pageNumber, 10);
            }
        }
    }
}
