using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assessments.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace Assessments.Controllers
{
    public class HomeController : Controller
    {
        IHostingEnvironment _hostingEnvironment;
        private readonly AssessmentContext _context;

        public HomeController(AssessmentContext context, IHostingEnvironment env)
        {
            _hostingEnvironment = env;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ImportExport()
        {

            // var Assess = (from a in _context.Assess
            //           select a).ToList();


            AssessmentViewModel AVM = new AssessmentViewModel();
            //AVM.Assessments = Assess;

            return View(AVM);
        }

        [HttpPost]

        public async Task<JsonResult> Import()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);

            //string result = "failed";
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    if (headerRow == null) { return Json("failed"); }

                    int cellCount = headerRow.LastCellNum;

                    //sb.Append("<table class='table'><tr>");
                    for (int j = 0; j < cellCount; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                        //sb.Append("<th>" + cell.ToString() + "</th>");
                    }
                    //sb.Append("</tr>");
                    //sb.AppendLine("<tr>");
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        bool upt = false;
                        Assess_Info m = new Assess_Info();
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            //var r = _context.Assess.Where(c => c.UIC == row.GetCell((j + 1)).ToString()).FirstOrDefault();
                            //if (r == null && upt == false)
                            //{
                                if (j == 1) m.UIC = row.GetCell(j).ToString();
                                if (j == 2) m.CTEUIC = row.GetCell(j).ToString();
                                if (j == 3) m.CIPCode = row.GetCell(j).ToString();
                                if (j == 4) m.OBNo = row.GetCell(j).ToString();
                                if (j == 5) m.SchoolName = row.GetCell(j).ToString();
                                if (j == 6) m.FANo = row.GetCell(j).ToString();
                                if (j == 7) m.TestCode = row.GetCell(j).ToString();
                                if (j == 8) m.TestName = row.GetCell(j).ToString();
                                try
                                {
                                    if (j == 9) m.TestDate = Convert.ToDateTime(row.GetCell(j).ToString());
                                }
                                catch (Exception ex)
                                {
                                    var mx = ex;
                                }
                                if (j == 10) m.Teacher = row.GetCell(j).ToString();
                                if (j == 11) m.Cluster = row.GetCell(j).ToString();
                                if (j == 12) m.LastName = row.GetCell(j).ToString();
                                if (j == 13) m.FirstName = row.GetCell(j).ToString();
                                try { if (j == 14) m.AssessScore = Convert.ToDecimal(row.GetCell(j).ToString()); }
                                catch (Exception ex) { var mx = ex; }
                                if (j == 15) m.AssessYear = row.GetCell(j).ToString();
                                if (j == 16) m.PassFail = row.GetCell(j).ToString();
                                try
                                {
                                    if (j == 17)
                                    {
                                        if (row.GetCell(j) == null) { m.UpdateDate = new DateTime(1 / 1 / 0001); }
                                        m.UpdateDate = Convert.ToDateTime(row.GetCell(j).ToString());
                                    }
                                }
                                catch (Exception ex) { var mx = ex; }
                                if (j == 18) m.UpdateBy = row.GetCell(j).ToString();
                                if (j == 19) m.Comment = row.GetCell(j).ToString();
                                try { if (j == 20) m.encLastName = Convert.ToByte(row.GetCell(j).ToString()); }
                                catch (Exception ex) { var mx = ex; }
                                try { if (j == 21) m.encFirstName = Convert.ToByte(row.GetCell(j).ToString()); }
                                catch (Exception ex) { var mx = ex; }
                                try { if (j == 22) m.Accepted = Convert.ToChar(row.GetCell(j).ToString()); }
                                catch (Exception ex) { var mx = ex; }
                                if (j >= 23)
                                {
                                    _context.Add(m);
                                    await _context.SaveChangesAsync();
                                }

                            //}
                            //else if (upt == false)
                            //{
                            //    r.CTEUIC = row.GetCell(2).ToString();
                            //    r.CIPCode = row.GetCell(3).ToString();
                            //    r.OBNo = row.GetCell(4).ToString();
                            //    r.SchoolName = row.GetCell(5).ToString();
                            //    r.FANo = row.GetCell(6).ToString();
                            //    r.TestCode = row.GetCell(7).ToString();
                            //    r.TestName = row.GetCell(8).ToString();
                            //    try
                            //    {
                            //        r.TestDate = Convert.ToDateTime(row.GetCell(9).ToString());
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        var mx = ex;
                            //    }
                            //    r.Teacher = row.GetCell(10).ToString();
                            //    r.Cluster = row.GetCell(11).ToString();
                            //    r.LastName = row.GetCell(12).ToString();
                            //    r.FirstName = row.GetCell(13).ToString();
                            //    try { r.AssessScore = Convert.ToDecimal(row.GetCell(14).ToString()); }
                            //    catch (Exception ex) { var mx = ex; }
                            //    r.AssessYear = row.GetCell(15).ToString();
                            //    r.PassFail = row.GetCell(16).ToString();
                            //    try
                            //    {

                            //        {
                            //            if (row.GetCell(17) == null) { r.UpdateDate = new DateTime(1 / 1 / 0001); }
                            //            else { r.UpdateDate = Convert.ToDateTime(row.GetCell(17).ToString()); }
                            //        }
                            //    }
                            //    catch (Exception ex) { var mx = ex; }
                            //    r.UpdateBy = row.GetCell(18).ToString();
                            //    r.Comment = row.GetCell(19).ToString();
                            //    try { r.encLastName = Convert.ToByte(row.GetCell(20).ToString()); }
                            //    catch (Exception ex) { var mx = ex; }
                            //    try { r.encFirstName = Convert.ToByte(row.GetCell(21).ToString()); }
                            //    catch (Exception ex) { var mx = ex; }
                            //    try { r.Accepted = Convert.ToChar(row.GetCell(22).ToString()); }
                            //    catch (Exception ex) { var mx = ex; }
                            //    _context.Update(r);
                            //    _context.SaveChanges();
                            //    upt = true;
                            //}



                        }

                    }
                }
            }
            var Assess = (from a in _context.Assess
                          select a).ToList();
            AssessmentViewModel AVM = new AssessmentViewModel();
            AVM.Assessments = Assess;

            return Json(AVM);
        }
    }
}
