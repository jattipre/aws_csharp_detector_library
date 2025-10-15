using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MAYShop.Models;
using PagedList;

namespace MAYShop.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        // GET: Admin/Brand
        MayShopDataContext db = new MayShopDataContext();
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.Brands.ToList().ToPagedList(iPageNum,iPageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Brand b, FormCollection f)
        {
            b.Name = f["sName"];
            db.Brands.InsertOnSubmit(b);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var nxb = db.Brands.SingleOrDefault(n => n.BrandID == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nxb);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var nxb = db.Brands.SingleOrDefault(n => n.BrandID == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Brands.DeleteOnSubmit(nxb);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sach = db.Brands.SingleOrDefault(n => n.BrandID == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var nxb = db.Brands.SingleOrDefault(n => n.BrandID == int.Parse(f["iMaNXB"]));
            if (ModelState.IsValid)
            {
                nxb.Name = f["sName"];
                db.SubmitChanges();
                return RedirectToAction("Index");

            }
            return View(nxb);
        }
    }
}