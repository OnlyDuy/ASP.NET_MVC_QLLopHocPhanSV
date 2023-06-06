using QLLopHocPhanSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Data;

namespace QLLopHocPhanSV.Controllers
{
    public class LopHPController : Controller
    {
        QLLHSVDataContext db = new QLLHSVDataContext();
        // GET: LopHP
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetListLHP()
        {
            List<tbl_LopHocPhan> list = new List<tbl_LopHocPhan>();
            list = db.tbl_LopHocPhans.ToList();
            return Content(JsonConvert.SerializeObject(list));
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult PostCreate()
        {
            Result_ett<string> rs = new Result_ett<string>();
            try
            {
                string maLop = Request["MaLop"];
                string tenLop = Request["TenLop"];
                string sotinchi = Request["SoTinChi"];

                // Kiểm tra các trường không được để trống
                if (string.IsNullOrEmpty(maLop) || string.IsNullOrEmpty(tenLop) || string.IsNullOrEmpty(sotinchi))
                {
                    rs.ErrCode = EnumErrCode.Fail;
                    rs.Message = "Các trường không được để trống";
                }
                else
                {
                    // Kiểm tra xem MSSV đã tồn tại trong cơ sở dữ liệu chưa
                    int isExist = db.tbl_LopHocPhans.Count(lhp => lhp.MaLop == maLop);

                    if (isExist == 1)
                    {
                        rs.ErrCode = EnumErrCode.Fail;
                        rs.Message = "Mã lớp đã tồn tại";
                    }
                    else
                    {
                        tbl_LopHocPhan newLHP = new tbl_LopHocPhan();
                        newLHP.MaLop = maLop;
                        newLHP.TenLop = tenLop;
                        newLHP.SoTinChi = int.Parse(sotinchi);
                        // Gọi đến bảng sinh viên và insert dữ liệu
                        db.tbl_LopHocPhans.InsertOnSubmit(newLHP);
                        // Lưu thay đổi
                        db.SubmitChanges();

                        // Hiển thị success Thêm thành công
                        rs.ErrCode = EnumErrCode.Success;
                        rs.Message = "Thêm mới lớp thành công";
                    }
                }
            }
            catch (Exception ex)
            {
                // Hiển thị success Thêm thất bại
                rs.ErrCode = EnumErrCode.Fail;
                rs.ErrDetail = ex.Message;
                rs.Message = "Thêm mới lớp học phần thất bại";
                return Content(JsonConvert.SerializeObject(rs));
            }
            return Content(JsonConvert.SerializeObject(rs));
        }
    }
}