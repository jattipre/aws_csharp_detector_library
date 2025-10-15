using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BIDV.Common;
using BIDV.Model;
using BIDV.Repository;
using PagedList;

namespace BIDV.Controllers
{
    [Authorize]
    public class AdminQaController : Controller
    {
        //
        // GET: /AdminQa/
        readonly QaRepository _qaRepository = new QaRepository();
        readonly CategoryRepository _categoryRepository = new CategoryRepository();
        public ActionResult Index(string question, string answer,int id = 0, int category = 0, int page = 1)
        {
            ViewBag.question = question;
            ViewBag.answer = answer;
            ViewBag.category = category;
            var lstCategoryQa = _categoryRepository.GetWhere(g => g.type == (int)Config.TypeCategory.Qa);
            ViewBag.ListCategory = lstCategoryQa.ToList();
            var lstQa = _qaRepository.GetAll();
            if (id > 0)
            {
                lstQa = lstQa.Where(g => g.id == id);
            }
            else
            {
                if (category > 0)
                {
                    lstQa = lstQa.Where(g => g.cat_id == category);
                }
                if (!string.IsNullOrEmpty(question))
                {
                    lstQa =
                        lstQa.Where(
                            g => !string.IsNullOrEmpty(g.question) && 
                                HelperString.UnsignCharacter(g.question.ToLower().Trim())
                                    .Contains(HelperString.UnsignCharacter(question.ToLower().Trim())));
                }
                if (!string.IsNullOrEmpty(answer))
                {
                    lstQa =
                        lstQa.Where(
                            g => !string.IsNullOrEmpty(g.answer) && 
                                HelperString.UnsignCharacter(g.answer.ToLower().Trim())
                                    .Contains(HelperString.UnsignCharacter(answer.ToLower().Trim())));
                }
            }
            lstQa = lstQa.OrderByDescending(g => g.created);
            return View(lstQa.ToPagedList(page, Config.PageSize));
        }

        public ActionResult Add()
        {
            var lstCategoryQa = _categoryRepository.GetWhere(g => g.type == (int)Config.TypeCategory.Qa);
            ViewBag.ListCategory = lstCategoryQa.ToList();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(bidv__faqs item)
        {
            if (item.cat_id <= 0 || string.IsNullOrEmpty(item.question))
            {
                return RedirectToAction("Add","AdminQa");
            }
            item.created = (int?)HelperDateTime.Convert2TimeStamp(DateTime.Now);
            item.lang = "vi";
            item.status = 1;
            _qaRepository.Add(item);
            return RedirectToAction("Index", "AdminQa");
        }

        public ActionResult Edit(int id)
        {
            var lstCategoryQa = _categoryRepository.GetWhere(g => g.type == (int)Config.TypeCategory.Qa);
            ViewBag.ListCategory = lstCategoryQa.ToList();
            var objQa = _qaRepository.GetById(id);
            return View(objQa);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(bidv__faqs item)
        {
            if (item.cat_id <= 0 || string.IsNullOrEmpty(item.question))
            {
                return RedirectToAction("Edit","AdminQa", new {id = item.id});
            }
            _qaRepository.Update(item);
            return RedirectToAction("Index", "AdminQa");
        }

        public ActionResult Delete(int id)
        {
            var obj = _qaRepository.GetById(id);
            _qaRepository.Delete(obj);
            return RedirectToAction("Index", "AdminQa");
        }
    }
}
