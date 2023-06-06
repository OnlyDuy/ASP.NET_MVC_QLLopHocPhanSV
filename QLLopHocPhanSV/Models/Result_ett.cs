using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLLopHocPhanSV.Models
{
    public class Result_ett<T>
    {
        public EnumErrCode ErrCode { get; set; }
        public T Data { get; set; }
        public string ErrDescription { get; set; }
        public string ErrDetail { get; set; }
        public string Message { get; set; }

        public Result_ett()
        {
            // Ban đầu Result_ett khởi tạo thì ErrCode = 0 là Error
            ErrCode = EnumErrCode.Error;

            // Chuỗi khởi tạo rỗng
            ErrDescription = string.Empty;

            // Detail cũng rỗng
            ErrDetail = string.Empty;
        }
    }
}