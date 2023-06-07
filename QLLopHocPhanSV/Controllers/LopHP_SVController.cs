using QLLopHocPhanSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace QLLopHocPhanSV.Controllers
{
    public class LopHP_SVController : Controller
    {
        QLLHSVDataContext db = new QLLHSVDataContext();
        // GET: LopHP_SV
        public ActionResult getAddSV(string malop)
        {
            ViewBag.MaLop = malop;
            return View();
        }

        public ActionResult postAddSV(string listmssv)
        {
            var rs = JsonConvert.DeserializeObject<string[]>(listmssv)[0];
            //Result_ett<string> rse = new Result_ett<string>();

            //string maLop = Request["MaLop"];
            //string mSSV = Request["MSSV"];
            //string hoTen = Request["HoTen"];

            //tbl_LopHocPhan_SinhVien newLHP_SV = new tbl_LopHocPhan_SinhVien();
            //newLHP_SV.MaLop = maLop;
            //newLHP_SV.MSSV = mSSV;
            //newLHP_SV.HoTen = hoTen;
            //// Gọi đến bảng sinh viên và insert dữ liệu
            //db.tbl_LopHocPhan_SinhViens.InsertOnSubmit(newLHP_SV);
            //// Lưu thay đổi
            //db.SubmitChanges();

            return Content(rs);
        }
    }
}