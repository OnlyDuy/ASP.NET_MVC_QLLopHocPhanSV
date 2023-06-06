using QLLopHocPhanSV.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Security.Cryptography;

namespace QLLopHocPhanSV.Controllers
{
    public class SinhVienController : Controller
    {
        // Tạo 1 đối tượng để cung cấp quyền truy cập và tương tác với cơ sở dữ liệu.
        QLLHSVDataContext db = new QLLHSVDataContext();

        // GET: SinhVien phương thức dùng để hiển thị trang chủ view của ứng dụng
        public ActionResult Index()
        {
            return View();
        }


        //Phương thúc lấy ra danh sách sinh viên dựa vào tham số truyền vào
        public ActionResult GetListSV(string searchVal, int sortType, int pageSize, int currentPage)
        {
            // Tạo biến list lấy ra danh sách sinh viên từ cơ sở dữ liệu thông qua đối tượng db 
            // Contains: kiểm trả chuỗi con trong chuỗi này có xuất hiện trong chuỗi cha chứa nó hay không
            var list = db.tbl_SinhViens.Where(sv => (sv.MSSV.Contains(searchVal) || sv.HoTen.Contains(searchVal)));
            if (sortType == 0)
            {
                // sortTyoe là kiểu sắp xếp
                // Mặc định là 0 thì sắp xếp theo MSSV
                list = list.OrderBy(sv => sv.MSSV);
            }
            else
            {
                // Ngược lại sắp xếp theo HoTen
                list = list.OrderBy(sv => sv.HoTen);
            }

            // Hàm tính Số trang = Tổng số sinh viên / số sinh viên trong 1 trang
            // Ép kiểu về double và làm tròn bằng phương thức Math.Ceiling
            int totalPage = (int)Math.Ceiling((double)list.Count() / pageSize);

            // Lấy ra dssv được hiển thị trong trang hiện tại
            // Skip: sử dụng để bỏ qua các phần tử trong ds theo số lượng phần tử cần bỏ qua
            // Take: lấy số lượng phần tử cần phải lấy
            var dssv = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            // kết quả trả về là 1 đối tượng JSON
            return Content(JsonConvert.SerializeObject(new { dssv, totalPage }));   
        }


        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        // Hàm lấy thông tin Sinh viên theo mssv
        public string GetObject(string mssv)
        {
            // CHuyển đối tượng về dạng Object bằng cách sd JSON
            tbl_SinhVien sinhvien = new tbl_SinhVien();
            sinhvien = db.tbl_SinhViens.Where(o => o.MSSV.Equals(mssv)).FirstOrDefault();
            return JsonConvert.SerializeObject(sinhvien);
        }
        public string PostCreate()
        {
            Result_ett<string> rs = new Result_ett<string>();
            //xử lý thêm
            try
            {
                string hoTen = Request["HoTen"];
                string mSSV = Request["Mssv"];
                string diaChi = Request["DiaChi"];
                string khoaHoc = Request["KhoaHoc"];
                string lopQuanLy = Request["LopQuanLy"];
                string ngaySinh = Request["NgaySinh"];
                string gioiTinh = Request["GioiTinh"];

                // Kiểm tra các trường không được để trống
                if (string.IsNullOrEmpty(mSSV) || string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(diaChi) || string.IsNullOrEmpty(khoaHoc) || string.IsNullOrEmpty(lopQuanLy) || string.IsNullOrEmpty(ngaySinh) || string.IsNullOrEmpty(gioiTinh))
                {
                    rs.ErrCode = EnumErrCode.Fail;
                    rs.Message = "Các trường không được để trống";
                }
                else
                {
                    // Kiểm tra xem MSSV đã tồn tại trong cơ sở dữ liệu chưa
                    int isExist = db.tbl_SinhViens.Count(sv => sv.MSSV == mSSV);

                    if (isExist == 1)
                    {
                        rs.ErrCode = EnumErrCode.Fail;
                        rs.Message = "Mã số sinh viên đã tồn tại";
                    }
                    else
                    {
                        tbl_SinhVien newSV = new tbl_SinhVien();
                        newSV.MSSV= mSSV;
                        newSV.HoTen = hoTen;
                        newSV.KhoaHoc = khoaHoc;
                        newSV.LopQuanLy = lopQuanLy;
                        newSV.DiaChi = diaChi;
                        newSV.NgaySinh = DateTime.ParseExact(ngaySinh, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        newSV.GioiTinh = gioiTinh;
                        // Gọi đến bảng sinh viên và insert dữ liệu
                        db.tbl_SinhViens.InsertOnSubmit(newSV);
                        // Lưu thay đổi
                        db.SubmitChanges();

                        // Hiển thị success Thêm thành công
                        rs.ErrCode = EnumErrCode.Success;
                        rs.Message = "Thêm mới sinh viên thành công";
                    }
                }
            }
            catch(Exception ex)
            {
                // Hiển thị success Thêm thất bại
                rs.ErrCode = EnumErrCode.Fail;
                rs.ErrDetail = ex.Message;
                rs.Message = "Thêm mới sinh viên thất bại";
                return JsonConvert.SerializeObject(rs);
            }
            return JsonConvert.SerializeObject(rs);
        }

        // Sửa, cần tạo 1 cái Form Sửa,
        // khi click vào Button sửa thì thông tin dữ liệu sẽ đẩy vào form sửa đó
       
        public string PostEdit()
        {
            Result_ett<string> rs = new Result_ett<string>();
            //xử lý cập nhật
            try
            {
                string hoTen = Request["HoTen"];
                string mSSV = Request["Mssv"];
                string diaChi = Request["DiaChi"];
                string khoaHoc = Request["KhoaHoc"];
                string lopQuanLy = Request["LopQuanLy"];
                string ngaySinh = Request["NgaySinh"];
                string gioiTinh = Request["GioiTinh"];

                tbl_SinhVien newSV = db.tbl_SinhViens.Where(o => o.MSSV.Equals(mSSV)).FirstOrDefault();
                newSV.HoTen = hoTen;
                newSV.KhoaHoc = khoaHoc;
                newSV.LopQuanLy = lopQuanLy;
                newSV.DiaChi = diaChi;
                newSV.NgaySinh = DateTime.ParseExact(ngaySinh, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                newSV.GioiTinh = gioiTinh;
       
                // Lưu thay đổi
                db.SubmitChanges();

                // Hiển thị success Thêm thành công
                rs.ErrCode = EnumErrCode.Success;
                rs.Message = "Cập nhật sinh viên thành công";
            }
            catch (Exception ex)
            {
                // Hiển thị success Thêm thất bại
                rs.ErrCode = EnumErrCode.Fail;
                rs.ErrDetail = ex.Message;
                rs.Message = "Cập nhật sinh viên thất bại";
                return JsonConvert.SerializeObject(rs);
            }
            return JsonConvert.SerializeObject(rs);
        }

        // Xóa thì tạo ra 1 alert confirm
        public string Delete(string mssv)
        {
            Result_ett<string> rs = new Result_ett<string>();
            try
            {
                tbl_SinhVien delObj = db.tbl_SinhViens.Where(o => o.MSSV.Equals(mssv)).FirstOrDefault();
                db.tbl_SinhViens.DeleteOnSubmit(delObj);
                db.SubmitChanges();
                rs.ErrCode = EnumErrCode.Success;
                rs.Message = "Xóa sinh viên thành công";
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Fail;
                rs.ErrDetail = ex.Message;
                rs.Message = "Xóa sinh viên không thành công";
                return JsonConvert.SerializeObject(rs);
            }
            return JsonConvert.SerializeObject(rs);
        }

    }
}