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
            var rs = JsonConvert.DeserializeObject<string[]>(listmssv);

            foreach (var mssv in rs)
            {
                var sinhvien = db.tbl_SinhViens.FirstOrDefault(sv => sv.MSSV == mssv);

                if (sinhvien != null)
                {
                    string maLop = ViewBag.MaLop;
                    tbl_LopHocPhan_SinhVien newLHP_SV = new tbl_LopHocPhan_SinhVien();
                    newLHP_SV.MaLop = maLop;
                    newLHP_SV.MSSV = sinhvien.MSSV;
                    //newLHP_SV.HoTen = sinhvien.HoTen;
                    db.tbl_LopHocPhan_SinhViens.InsertOnSubmit(newLHP_SV);
                }
            }

            db.SubmitChanges();

            return Content(JsonConvert.SerializeObject(rs));
        }
    }
}