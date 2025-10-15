using BLL;
using Entity.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class BlogController : Controller
    {
        
        // GET: Blog
        public ActionResult Index()
        {
            ArticleRepository aRep = new ArticleRepository();
            return View(aRep.GetAllArticlesOrderByDate());
        }
        public ActionResult Tags(string id)
        {
            ViewBag.Etiket = id;
            ArticleRepository aRep = new ArticleRepository();
            var model = aRep.GetAllArticles().Where(x => x.Tags.Contains(id)).ToList();
            return View(model);
        }

        [Authorize(Roles ="Admin,Author")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        
        [Authorize(Roles = "Admin,Author")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Article article, HttpPostedFileBase Resim, HttpPostedFileBase Resim2, HttpPostedFileBase Resim3)
        {
            article.AuthorID = User.Identity.Name;
            article.Picture = FileUpload(Resim);
            article.Picture2 = FileUpload(Resim2);
            article.Picture3 = FileUpload(Resim3);
            if (ModelState.IsValid)
            {
                new ArticleRepository().AddArticle(article);
            }
            
            return View();
        }
        private string FileUpload(HttpPostedFileBase file)
        {

            if (file != null)
            {
                string rastgele = Guid.NewGuid().ToString();
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("/Content/Upload/" + rastgele + ImageName);
                string resim = "/Content/Upload/" + rastgele + ImageName;
                //// save image in folder
                file.SaveAs(physicalPath);
                return resim;
            }
            return "";
        }
        public ActionResult Detail(int id)
        {
            var article = new ArticleRepository().GetArticleByID(id);
            ViewBag.Keywords = article.Tags;
            return View(new ArticleRepository().GetArticleByID(id));
        }
        [Authorize(Roles="Admin")]
        public JsonResult Delete(int id)
        {
            try
            {
                new ArticleRepository().DeleteArticleByID(id);
                return Json(new {
                    message = "Article is deleted.",
                    status = true
                });
            }
            catch(Exception ex){ return Json(new {message = "Oops! Error: "+ex.Message, status = false }); }
        }
        public JsonResult VoteArticle(int id, int points)
        {
            ArticleRepository rep = new ArticleRepository();
            try
            {
                if (Session["HasVoted_" + id]==null || (bool)Session["HasVoted_" + id]!=true)
                {
                    var article = new ArticleRepository().GetArticleByID(id);
                    article.TotalRate += points;
                    rep.UpdateArticle(article);
                    Session["HasVoted_" + id] = true;
                    return Json("Oy Verdiğiniz için teşekkürler");
                }
                else
                {
                    return Json("Tekrar oy veremezsiniz.");
                }
                
                
            }
            catch (Exception ex)
            {
                return Json("Hata - " + ex.Message);
            }
        }

        public ActionResult SubscriptionBox()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubscriptionBox(string Email)
        {
            var a = Request.Form;
            EmailSubscriptionRepository erep = new EmailSubscriptionRepository();
            erep.AddEmailSubscription(new EmailSubscription() { Email = Email });
            //new EmailSubscriptionRepository().AddEmailSubscription(Email);
            return View();
        }
    }
}