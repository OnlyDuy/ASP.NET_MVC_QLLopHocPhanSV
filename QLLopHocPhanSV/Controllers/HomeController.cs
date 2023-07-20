using QLLopHocPhanSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLLopHocPhanSV.Controllers
{
    public class HomeController : Controller
    {
        QLLHSVDataContext db = new QLLHSVDataContext();

        public ActionResult Index()
        {
            return View();
        }

        // HTTP get /Home/Register
        public ActionResult Register()
        {
            return View();
        }

        // HTTP post /Home/Register
        [HttpPost]
        public ActionResult Register(tbl_NguoiDung nguoiDung)
        {
            // Kiểm tra xem Username, SDT và DiaChi đã tồn tại trong database hay chưa
            var isUsernameExists = db.tbl_NguoiDungs.Any(u => u.Username == nguoiDung.Username);
            var isSDTExists = db.tbl_NguoiDungs.Any(u => u.SDT == nguoiDung.SDT);
            var isDiaChiExists = db.tbl_NguoiDungs.Any(u => u.DiaChi == nguoiDung.DiaChi);


            if (isUsernameExists || isSDTExists || isDiaChiExists)
            {
                if (isUsernameExists)
                {
                    ViewBag.LoginFail("Username", "Tên người dùng đã tồn tại trong hệ thống.");
                }
                if (isSDTExists)
                {
                    ViewBag.LoginFail("SDT", "Số điện thoại đã tồn tại trong hệ thống.");
                }
                if (isDiaChiExists)
                {
                    ViewBag.LoginFail("DiaChi", "Địa chỉ đã tồn tại trong hệ thống.");
                }

                // Trả về view đăng ký cùng với các thông báo lỗi
                return View("Register");
            }

            db.tbl_NguoiDungs.InsertOnSubmit(nguoiDung);
            db.SubmitChanges();
            return RedirectToAction("Login");
        }

        // HTTP get /Home/Login
        public ActionResult Login()
        {
            return View();
        }

        // HTTP post /Home/Login
        [HttpPost]
        public ActionResult Login(tbl_NguoiDung nguoiDung)
        {
            var usernameForm = nguoiDung.Username;
            var passwordForm = nguoiDung.Password;
            var userCheck = db.tbl_NguoiDungs.SingleOrDefault(x=>x.Username.Equals(usernameForm) && x.Password.Equals(passwordForm));

            if (userCheck != null)
            {
                Session["tbl_NguoiDung"] = userCheck;
                return RedirectToAction("Index", "Home");       
            }
            else
            {
                ViewBag.LoginFail = "Đăng nhập thất bại, vui lòng kiểm tra lại";
            } 
            return View("Login");
        }

        // HTTP post /Home/Logout
        public ActionResult Logout()
        {
            Session["tbl_NguoiDung"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}