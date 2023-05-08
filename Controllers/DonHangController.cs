using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteThuCungCute.App_Start;
using WebsiteThuCungCute.Models;
namespace WebsiteThuCungCute.Controllers
{
    [AdminPhanQuyen(MACHUCNANG = "QL_DONDATHANG")]
    public class DonHangController : Controller
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        // GET: QLDonDatHang
        public Double iTONGTIEN { set; get; }
        public Double tTONGTIEN { set; get; }
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("dangnhap", "Admin");
            }
            else
            {
                return View();
            }
        }


        public ActionResult DonDatHang()
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("dangnhap", "Admin");
            }
            else
            {
                var hang = from h in data.DONDATHANGs select h;
                return View(hang);
            }
        }

        //private double TongTien()
        //{
        //    double iTongTien = 0;
        //    List<Giohang> dsGiohang = Session["GioHang"] as List<Giohang>;
        //    if (dsGiohang != null)
        //    {
        //        iTongTien = dsGiohang.Sum(n => n.dTHANHTIEN);
        //    }
        //    return iTongTien;
        //}

        public ActionResult ChiTietDonHang(int id)
        {
            if (Session["Taikhoanadmin"] == null)//Chưa đăng nhập => Login
            {
                return RedirectToAction("dangnhap", "Admin");
            }    
            else
            { 
            //Lấy ra thông tin Chi tiết đơn hàng từ mã đơn hàng truyền vào
            //Ở đây 1 đơn hàng có thể có nhiều chi tiết ĐH(mua nhiều SP), nên dùng where như trang sản phẩm theo nhà sản xuất
            var CTDH = (from c in data.CTDONDATHANGs where c.MADH == id select c).ToList();
                return View(CTDH);
            }
        }

        [HttpGet]
        public ActionResult XoaDonHang(int id)
        {
            DONDATHANG hang = data.DONDATHANGs.SingleOrDefault(h => h.MADH == id);
            //ViewBag.MaHang = hang.MaHang;
            //if (hang == null)
            //{
            //    Response.StatusCode = 404;
            //    return null;
            //}
            return View(hang);
        }

        [HttpPost, ActionName("Xóa đơn hàng")]
        public ActionResult XacnhanXoaDon(int id)
        {
            DONDATHANG hang = data.DONDATHANGs.SingleOrDefault(h => h.MADH == id);
            data.DONDATHANGs.DeleteOnSubmit(hang);
            data.SubmitChanges();
            return RedirectToAction("DonDatHang", "DonHang");
        }
    }
}