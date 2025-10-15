using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using System.Net.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityLayer.Concrete;
using BusinessLayer.ValidationRules;
using FluentValidation.Results;

namespace MvcBlogProje.Controllers
{
    public class MessageController : Controller
    {
        // GET: Message
        MessageManager mm = new MessageManager(new EfMessageDal());
        MessageValidator messagevalidar = new MessageValidator();
        public ActionResult Inbox(string search)
        {
            //var messagelist = mm.GetListInbox((string)Session["AdminUserName"]);
            //return View(messagelist);
            if (string.IsNullOrEmpty(search))
            {
                ViewBag.search = "";
                var messagelist = mm.GetListInbox((string)Session["AdminUserName"]);
                return View(messagelist);
            }
            else
            {
                ViewBag.search = search;
                var messagelist = mm.GetListInbox((string)Session["AdminUserName"], search);
                return View(messagelist);
            }
        }
        public ActionResult Sendbox(string search)
        {
            //var messagelist = mm.GetListSendbox((string)Session["AdminUserName"]);
            //return View(messagelist);
            if (string.IsNullOrEmpty(search))
            {
                ViewBag.search = "";
                var messagelist = mm.GetListInbox((string)Session["AdminUserName"]);
                return View(messagelist);
            }
            else
            {
                ViewBag.search = search;
                var messagelist = mm.GetListInbox((string)Session["AdminUserName"], search);
                return View(messagelist);
            }
        }

        public ActionResult GetMessageDetails(int id)
        {
            var value = mm.GetById(id);
            value.MessageRead = true;
            mm.MessageUpdateBL(value);
            return View(value);
        }

        public ActionResult GetSendMessageDetails(int id)
        {
            var value = mm.GetById(id);
            value.MessageRead = true;
            mm.MessageUpdateBL(value);
            return View(value);
        }

        [HttpGet]
        public ActionResult NewMessage()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewMessage(Message message)
        {
            ValidationResult sonuc = messagevalidar.Validate(message);
            message.SenderMail = "admin@gmail.com";
            message.MessageDate = DateTime.Now; //Şuanki tarihi MessageDate'e aktarma
            if (sonuc.IsValid)
            {
                mm.MessageAddBL(message);
                return RedirectToAction("Sendbox");
            }
            else
            {
                foreach (var item in sonuc.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }
    }
}