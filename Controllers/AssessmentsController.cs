using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Assessments.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Assessments.Controllers
{
    public class AssessmentsController : Controller
    {
        IHostingEnvironment _hostingEnvironment;
        private readonly AssessmentContext _context;
        public AssessmentsController(AssessmentContext context, IHostingEnvironment env)
        {
            _hostingEnvironment = env;
            _context = context;
        }
        public IActionResult Index()
        {
            FAviewModel FA = new FAviewModel();

            //FA.FAname.Add("huroun high school");
            //FA.FAname.Add("Easst Lansing high school"); 
            //FA.FAname.Add("Lansing high school"); 
            //FA.FAname.Add("Capital Area high school"); 

            var Assess = (from a in _context.Assess
                          select a.TestName).Distinct().ToList();


            FA.FAname = Assess;

            return View(FA);
        }

        public JsonResult DropDown1(string testname, bool makesure)
        {
            AssessmentViewModel AVM = new AssessmentViewModel();
            var Assess = (from a in _context.Assess
                          select a).Where(c => c.TestName == testname).Distinct().ToList();
            AVM.Assessments = Assess;
            return Json(AVM);
        }
        [HttpPost]
        public async Task<IActionResult> Export(string testname)
        {
            var Ex = (from a in _context.Assess
                      select a).Where(c => c.TestName == testname).Distinct().ToList();

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"demo.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Demo");
                IRow row = excelSheet.CreateRow(0);


                for (int i = 0; i < Ex.Count; i++)
                {
                    row.CreateCell(0).SetCellValue(Ex[i].RecId);
                    row.CreateCell(1).SetCellValue(Ex[i].UIC);
                    row.CreateCell(2).SetCellValue(Ex[i].CTEUIC);
                    row.CreateCell(3).SetCellValue(Ex[i].CIPCode);
                    row.CreateCell(4).SetCellValue(Ex[i].OBNo);
                    row.CreateCell(5).SetCellValue(Ex[i].SchoolName);
                    row.CreateCell(6).SetCellValue(Ex[i].FANo);
                    row.CreateCell(7).SetCellValue(Ex[i].TestCode);
                    row.CreateCell(8).SetCellValue(Ex[i].TestName);
                    row.CreateCell(9).SetCellValue(Ex[i].TestDate);
                    row.CreateCell(10).SetCellValue(Ex[i].Teacher);
                    row.CreateCell(11).SetCellValue(Ex[i].Cluster);
                    row.CreateCell(12).SetCellValue(Ex[i].LastName);
                    row.CreateCell(13).SetCellValue(Ex[i].FirstName);
                    row.CreateCell(14).SetCellValue(Ex[i].AssessScore.ToString());
                    row.CreateCell(15).SetCellValue(Ex[i].AssessYear);
                    row.CreateCell(16).SetCellValue(Ex[i].PassFail);
                    row.CreateCell(17).SetCellValue(Ex[i].UpdateDate);
                    row.CreateCell(18).SetCellValue(Ex[i].UpdateBy);
                    row.CreateCell(19).SetCellValue(Ex[i].Comment);
                    row.CreateCell(20).SetCellValue(Ex[i].encLastName);
                    row.CreateCell(21).SetCellValue(Ex[i].encFirstName);
                    row.CreateCell(22).SetCellValue(Ex[i].Accepted);
                    row = excelSheet.CreateRow(i + 1);
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }


    }
}