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
    [AdminPhanQuyen(MACHUCNANG = "QL_THUONGHIEU")]
    public class ThuongHieuController : Controller
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        // GET: ThuongHieu
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var loai = from l in data.THUONGHIEUs select l;
                return View(loai);
            }
            
        }
        public ActionResult Details(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var loai = from l in data.THUONGHIEUs where l.MATH == id select l;
                return View(loai.Single());
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
        public ActionResult Create(THUONGHIEU loai, HttpPostedFileBase fileUpload)
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
                        loai.LOGO = fileName;
                        data.THUONGHIEUs.InsertOnSubmit(loai);
                        data.SubmitChanges();
                    }

                }                            
                return RedirectToAction("Index", "ThuongHieu");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var loai = from l in data.THUONGHIEUs where l.MATH == id select l;
                return View(loai.Single());
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
                THUONGHIEU loai = data.THUONGHIEUs.SingleOrDefault(n => n.MATH == id);
                
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
                    loai.LOGO = fileName;
                    UpdateModel(loai);
                    data.SubmitChanges();
                    return RedirectToAction("Index", "ThuongHieu");
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
                var loai = from nxb in data.THUONGHIEUs where nxb.MATH == id select nxb;
                return View(loai.Single());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                THUONGHIEU th = data.THUONGHIEUs.SingleOrDefault(k => k.MATH == id);
                var sanphams = data.SANPHAMs.Where(d => d.MATH == id).ToList();

                foreach (var sp in sanphams)
                {
                    var phieunhapkhos = data.PHIEUNHAPKHOs.Where(c => c.MASP == sp.MASP).ToList();

                    foreach (var pn in phieunhapkhos)
                    {
                        data.PHIEUNHAPKHOs.DeleteOnSubmit(pn);
                    }

                    data.SANPHAMs.DeleteOnSubmit(sp);
                }

                data.THUONGHIEUs.DeleteOnSubmit(th);
                data.SubmitChanges();
                return RedirectToAction("Index", "ThuongHieu");
            }
        }



        //public ActionResult Xoa(int id)
        //{
        //    if (Session["Taikhoanadmin"] == null)
        //        return RedirectToAction("dangnhap", "Admin");
        //    else
        //    {
        //        // Lấy thông tin thương hiệu cần xóa
        //        THUONGHIEU th = data.THUONGHIEUs.SingleOrDefault(k => k.MATH == id);

        //        // Lấy danh sách sản phẩm liên quan đến thương hiệu
        //        var relatedProducts = data.SANPHAMs.Where(p => p.MATH == id);

        //        // Duyệt qua danh sách sản phẩm
        //        foreach (var product in relatedProducts)
        //        {
        //            // Lấy danh sách phiếu nhập kho liên quan đến sản phẩm
        //            var relatedEntries = data.PHIEUNHAPKHOs.Where(e => e.MASP == product.MASP);

        //            // Duyệt qua danh sách phiếu nhập kho
        //            foreach (var entry in relatedEntries)
        //            {
        //                // Xóa sản phẩm khỏi phiếu nhập kho
        //                data.PHIEUNHAPKHOs.DeleteOnSubmit(entry);
        //            }

        //            // Xóa sản phẩm
        //            data.SANPHAMs.DeleteOnSubmit(product);
        //        }

        //        // Xóa thương hiệu
        //        data.THUONGHIEUs.DeleteOnSubmit(th);

        //        // Lưu các thay đổi vào cơ sở dữ liệu
        //        data.SubmitChanges();

        //        return RedirectToAction("Index", "ThuongHieu");
        //    }
        //}





        //public ActionResult Xoa(int id)
        //{
        //    if (Session["Taikhoanadmin"] == null)
        //        return RedirectToAction("dangnhap", "Admin");
        //    else
        //    {
        //        THUONGHIEU nhaxuatban = data.THUONGHIEUs.SingleOrDefault(n => n.MATH == id);
        //        data.THUONGHIEUs.DeleteOnSubmit(nhaxuatban);
        //        data.SubmitChanges();
        //        return RedirectToAction("Index", "ThuongHieu");
        //    }
        //}
    }
}