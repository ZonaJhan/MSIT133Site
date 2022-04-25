using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT133Site.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MSIT133Site.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context;
        private readonly IWebHostEnvironment _host;
        public ApiController(DemoContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        public IActionResult Index(User user)
        {
            System.Threading.Thread.Sleep(10000);
            if (string.IsNullOrEmpty(user.UserName))
            {
                user.UserName = "Ajax";
            }
            return Content($"Hello {user.UserName}, You are {user.age} years old", "text/plain", System.Text.Encoding.UTF8);
            //return Content("Hello " + name + "I'm " + age, "text/plain");
        }


        public IActionResult CheckName(string txtName)
        {
            //DemoContext db = new DemoContext();
            //db.Members.Where(m => m.Name == name)
            //_context.Member.Where
            if (string.IsNullOrEmpty(txtName))
            {
                return Content("Please entry your name", "text/plain", System.Text.Encoding.UTF8);
            }
            Member mem = _context.Members.FirstOrDefault(m => m.Name == txtName);
            if (mem != null)
            {
                return Content("Name already exist", "text/plain", System.Text.Encoding.UTF8);
            }
            return Content("You can use the name", "text/plain", System.Text.Encoding.UTF8);
        }

        public IActionResult Register(Member member, IFormFile photo/*和input的name一樣*/)
        {
            //C:\API\MSIT133Site\MSIT133Site\wwwroot\uploads\
            //_host.WebRootPath==>C:\API\MSIT133Site\MSIT133Site\wwwroot

            //upload 檔案到資料夾中
            string uploadFolder = Path.Combine(_host.WebRootPath, "uploads", photo.FileName);
            using (var fileStream = new FileStream(uploadFolder, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }

            //圖檔轉成二進位memoryStream
            byte[] imgByte = null;
            using (MemoryStream stream = new MemoryStream())
            {
                photo.CopyTo(stream);
                imgByte = stream.ToArray();
            }
            //寫進database
            member.FileName = photo.FileName;
            member.FileData = imgByte;

            _context.Members.Add(member);
            _context.SaveChanges();

            //string info = $"{_host.WebRootPath}-${_host.ContentRootPath}";
            string info = $"{photo.FileName}-{photo.Length}-{photo.ContentType}";
            //string info = uploadFolder;
            return Content(info, "text/plain", System.Text.Encoding.UTF8);
            //return Content($"Hello {user.UserName}, You are {user.age} years old. Email:{user.email}", "text/plain", System.Text.Encoding.UTF8);
        }
        //get Cities
        public IActionResult City() 
        {
            //var cities = _context.Addresses.Select(c => c.City).Distinct();
            var cities = _context.Addresses.Select(c => new { c.City}).Distinct().OrderBy(c =>c.City);
            return Json(cities);
        }
        //get Districts
        public IActionResult Districts(string city)
        {
            var districts = _context.Addresses.Where(a=>a.City==city).Select(a => new { a.SiteId }).Distinct().OrderBy(a => a.SiteId);
            return Json(districts);
        }
        public IActionResult Roads(string district)
        {
            var roads = _context.Addresses.Where(a => a.SiteId == district).Select(a => new { a.Road }).Distinct().OrderBy(a => a.Road);
            return Json(roads);
        }
        public IActionResult GetImageByte(int id = 1)
        {
            Member member = _context.Members.Find(id);
            byte[] img = member.FileData;
            return File(img, "image/jpeg");
        }
    }
}
