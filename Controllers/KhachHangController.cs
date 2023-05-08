using WebsiteThuCungCute.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteThuCungCute.App_Start;

namespace WebsiteThuCungCute.Controllers
{
    [AdminPhanQuyen(MACHUCNANG = "QL_KHACHHANG")]
    public class KhachHangController : Controller
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        // GET: ThuongHieu

        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var kh = from k in data.KHACHHANGs select k;
                return View(kh);
            }

        }

        public ActionResult Details(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var kh = from l in data.KHACHHANGs where l.MAKH == id select l;
                return View(kh.Single());
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(KHACHHANG kh, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/img/"), fileName);
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            fileUpload.SaveAs(path);
                        }
                        kh.HINHANH = fileName;
                        data.KHACHHANGs.InsertOnSubmit(kh);
                        data.SubmitChanges();
                       
                    }

                }
                return RedirectToAction("Index", "KhachHang");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var kh = from l in data.KHACHHANGs where l.MAKH == id select l;
                return View(kh.Single());
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateInput(false)]
        public ActionResult Capnhat(int id, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MAKH == id);
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                else
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/img/"), fileName);
                    fileUpload.SaveAs(path);
                    kh.HINHANH = fileName;
                    UpdateModel(kh);
                    data.SubmitChanges();
                    return RedirectToAction("Index", "KhachHang");
                }                             
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var kh = from nxb in data.KHACHHANGs where nxb.MAKH == id select nxb;
                return View(kh.Single());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                // Get the customer record
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(k => k.MAKH == id);

                // Get all the orders for the customer
                var orders = data.DONDATHANGs.Where(o => o.MAKH == id).ToList();

                // Loop through each order and delete all related products
                foreach (var order in orders)
                {
                    var products = data.CTDONDATHANGs.Where(p => p.MADH == order.MADH).ToList();
                    data.CTDONDATHANGs.DeleteAllOnSubmit(products);
                }

                // Delete all the orders for the customer
                data.DONDATHANGs.DeleteAllOnSubmit(orders);

                // Delete the customer record
                data.KHACHHANGs.DeleteOnSubmit(kh);

                // Save changes to the database
                data.SubmitChanges();

                return RedirectToAction("Index", "KhachHang");
            }
        }
    }
}