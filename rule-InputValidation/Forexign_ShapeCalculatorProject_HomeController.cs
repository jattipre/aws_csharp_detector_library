using DataAccessLayer.Models;
using DevExpress.Web.Mvc;
using Microsoft.AspNet.Identity;
using PortalMVC2.SessionManager;
using System;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PortalMVC2.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult MyView()
        {
            var UserFullName = SessionDataManager.CurrentUser.FirstName + " " + SessionDataManager.CurrentUser.LastName;
            ViewBag.Name = ToTitleCase(UserFullName);
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult TotalGridViewPartial()
        {
            var userId = SessionDataManager.CurrentUser.UserID;
            var roles = userManager.GetRoles(userId);

            IQueryable<InputAndResultViewModel> model = null;

            if (User.IsInRole("Admin"))
            {
                model = db.Database.SqlQuery<InputAndResultViewModel>("SELECT * FROM InputAndResultView").AsQueryable();
            }
            else if (roles.Contains("DataInserter") || roles.Contains("DataCalculator"))
            {
                var usersInRoles = db.Users.ToList()
                    .Where(u => userManager.IsInRole(u.Id, "DataInserter") || userManager.IsInRole(u.Id, "DataCalculator"))
                    .Select(u => u.Id)
                    .ToList();

                model = db.Database.SqlQuery<InputAndResultViewModel>("SELECT * FROM InputAndResultView")
                    .Where(i => usersInRoles.Contains(i.UserId.ToString()))
                    .AsQueryable();
            }
            else // memberlar
            {
                model = db.Database.SqlQuery<InputAndResultViewModel>("SELECT * FROM InputAndResultView")
                    .Where(i => i.UserId == new Guid(userId))
                    .AsQueryable();
            }

            return PartialView("~/Views/Home/_TotalGridViewPartial.cshtml", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TotalGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] DataAccessLayer.Models.InputAndResultViewModel item)
        {
            if (!IsThereEmptyFields(item))
            {
                ViewData["EditError"] = "Lütfen şekil tipini ve gerekli alanları doldurunuz...";
                return TotalGridViewPartial();
            }

            var model = db.Inputs;
            if (ModelState.IsValid)
            {
                Inputs input = new Inputs()
                {
                    Id = item.Id,
                    ShapeType = item.ShapeType,
                    CreatedDate = DateTime.Now,
                    HesaplandiMi = false,
                    Kenar = item.Kenar,
                    KisaKenar = item.KisaKenar,
                    UzunKenar = item.UzunKenar,
                    Yaricap = item.Yaricap,
                    Yukseklik = item.Yukseklik,
                    UserId = new Guid(SessionDataManager.CurrentUser.UserID),
                };
                try
                {
                    model.Add(input);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return TotalGridViewPartial();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TotalGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] DataAccessLayer.Models.InputAndResultViewModel item)
        {
            var Inputmodel = db.Inputs;
            var ShapeResultModel = db.ShapeResults;

            if (ModelState.IsValid)
            {
                Inputs input = new Inputs()
                {
                    Id = item.Id,
                    ShapeType = item.ShapeType,
                    CreatedDate = DateTime.Now,
                    HesaplandiMi = false,
                    Kenar = item.Kenar,
                    KisaKenar = item.KisaKenar,
                    UzunKenar = item.UzunKenar,
                    Yaricap = item.Yaricap,
                    Yukseklik = item.Yukseklik,
                    UserId = new Guid(SessionDataManager.CurrentUser.UserID),
                };

                try
                {
                    var modelItem = Inputmodel.FirstOrDefault(it => it.Id == input.Id);
                    if (modelItem != null)
                    {
                        input.CreatedDate = DateTime.Now;
                        input.HesaplandiMi = false;
                        Inputmodel.AddOrUpdate(input);

                        var a = ShapeResultModel.Where(it => it.InputsId == input.Id).FirstOrDefault();
                        ShapeResultModel.Remove(a);

                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return TotalGridViewPartial();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TotalGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] int Id)
        {
            var model = db.Inputs;
            var model2 = db.ShapeResults;
            if (Id >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.Id == Id);
                    var item2 = model2.FirstOrDefault(it => it.InputsId == Id);

                    if (item != null)
                        model.Remove(item);

                    if (item2 != null)
                        model2.Remove(item2);

                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return TotalGridViewPartial();
        }

        [HttpPost]
        public async Task<ActionResult> SendAction()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var content = new StringContent(String.Empty, Encoding.UTF8, "application/json");

#if (DEBUG)
                    var response = await client.PostAsync("https://localhost:44310/api/input", content);
#else
                    var response = await client.PostAsync(ConfigurationManager.AppSettings["FIRST_API_ENDPOINT"], content);
#endif
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Success");
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        ViewBag.ErrorMessage = $"API isteği başarısız oldu. Hata: {response.StatusCode}, Detaylar: {errorContent}";
                        return View("Error");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                ViewBag.ErrorMessage = $"HTTP isteği sırasında bir hata oluştu: {e.Message}";
                return View("Error");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = $"Bir hata oluştu: {e.Message}";
                return View("Error");
            }
        }

        private bool IsThereEmptyFields(InputAndResultViewModel item)
        {
            if (item.ShapeType is null)
                return false;
            if (item.ShapeType == "Dikdortgen" && (!item.KisaKenar.HasValue || !item.UzunKenar.HasValue))
            {
                return false;
            }
            else if (item.ShapeType == "Kare" && !item.Kenar.HasValue)
            {
                return false;
            }
            else if (item.ShapeType == "Daire" && !item.Yaricap.HasValue)
            {
                return false;
            }
            else if (item.ShapeType == "Silindir" && (!item.Yaricap.HasValue || !item.Yukseklik.HasValue))
            {
                return false;
            }
            else if (item.ShapeType == "Kup" && !item.Kenar.HasValue)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private string ToTitleCase(string str)
        {
            // Kültürü belirterek TextInfo nesnesi oluştur
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            // Metni küçük harfe çevir ve kelimelere ayır
            string[] kelimeler = str.ToLower().Split(' ');

            // Her kelimenin baş harfini büyük yap
            for (int i = 0; i < kelimeler.Length; i++)
            {
                if (!string.IsNullOrEmpty(kelimeler[i]))
                {
                    kelimeler[i] = textInfo.ToTitleCase(kelimeler[i]);
                }
            }
            // Sonucu birleştir ve döndür
            return string.Join(" ", kelimeler);
        }
    }
}


//[ValidateInput(false)]
//public ActionResult TheGridViewPartial()
//{

//    var userId = SessionDataManager.CurrentUser.UserID;
//    var roles = userManager.GetRoles(userId);

//    IQueryable<Inputs> model;

//    if (User.IsInRole("Admin"))
//    {
//        model = db.Inputs;
//    }
//    else if (roles.Contains("DataInserter") || roles.Contains("DataCalculator"))
//    {
//        var usersInRoles = db.Users.ToList()
//            .Where(u => userManager.IsInRole(u.Id, "DataInserter") || userManager.IsInRole(u.Id, "DataCalculator"))
//            .Select(u => u.Id)
//            .ToList();

//        model = db.Inputs.Where(i => usersInRoles.Contains(i.UserId.ToString()));
//    }
//    else // memberlar
//    {
//        model = db.Inputs.Where(i => i.UserId == new Guid(userId));
//    }

//    return PartialView("~/Views/Home/_TheGridViewPartial.cshtml", model.ToList());
//}

//[HttpPost, ValidateInput(false)]
//public ActionResult TheGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] DataAccessLayer.Models.Inputs item)
//{
//    var isOK = IsModelStateValid(item);
//    if (isOK)
//    {
//        var model = db.Inputs;
//        if (ModelState.IsValid)
//        {
//            //initial values for new items
//            item.HesaplandiMi = false;
//            item.CreatedDate = DateTime.Now;
//            item.UserId = new Guid(User.Identity.GetUserId());

//            try
//            {
//                model.Add(item);
//                db.SaveChanges();
//            }
//            catch (Exception e)
//            {
//                ViewData["EditError"] = e.Message;
//            }
//        }
//        else
//            ViewData["EditError"] = "Please, correct all errors.";
//    }
//    else
//    {
//        ModelState.AddModelError("", "");
//    }
//    return TheGridViewPartial();

//}
//[HttpPost, ValidateInput(false)]
//public ActionResult TheGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] DataAccessLayer.Models.Inputs item)
//{

//    var model = db.Inputs;
//    if (ModelState.IsValid)
//    {
//        try
//        {
//            var modelItem = model.FirstOrDefault(it => it.Id == item.Id);
//            if (modelItem != null)
//            {
//                item.CreatedDate = DateTime.Now;
//                item.UserId = modelItem.UserId;
//                item.HesaplandiMi = false;
//                model.AddOrUpdate(item);
//                db.SaveChanges();
//            }
//        }
//        catch (Exception e)
//        {
//            ViewData["EditError"] = e.Message;
//        }
//    }
//    else
//        ViewData["EditError"] = "Please, correct all errors.";
//    return TheGridViewPartial();
//}
//[HttpPost, ValidateInput(false)]
//public ActionResult TheGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] int Id)
//{
//    var model = db.Inputs;
//    var model2 = db.ShapeResults;
//    if (Id >= 0)
//    {
//        try
//        {
//            var item = model.FirstOrDefault(it => it.Id == Id);
//            var item2 = model2.FirstOrDefault(it => it.InputsId == Id);

//            if (item != null)
//                model.Remove(item);

//            if (item2 != null)
//                model2.Remove(item2);

//            db.SaveChanges();
//        }
//        catch (Exception e)
//        {
//            ViewData["EditError"] = e.Message;
//        }
//    }
//    return TheGridViewPartial();
//}

//[ValidateInput(false)]
//public ActionResult ShapeResultGridViewPartial()
//{
//    var userId = SessionDataManager.CurrentUser.UserID;
//    var roles = userManager.GetRoles(userId);

//    IQueryable<ShapeResult> model;

//    if (User.IsInRole("Admin"))
//    {
//        model = db.ShapeResults;
//    }
//    else if (roles.Contains("DataInserter") || roles.Contains("DataCalculator"))
//    {
//        var usersInRoles = db.Users.ToList()
//            .Where(u => userManager.IsInRole(u.Id, "DataInserter") || userManager.IsInRole(u.Id, "DataCalculator"))
//            .Select(u => u.Id)
//            .ToList();

//        model = db.ShapeResults.Where(i => usersInRoles.Contains(i.UserId.ToString()));
//    }
//    else // memberlar
//    {
//        model = db.ShapeResults.Where(i => i.UserId == new Guid(userId));
//    }
//    return PartialView("~/Views/Home/_ShapeResultGridViewPartial.cshtml", model.ToList());
//}