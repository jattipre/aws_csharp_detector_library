using DataAccess;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PccuClub.WebAuth;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Web.Mvc;
using Utility;
using WebAuth.Entity;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.角色權限設定)]
    public class RoleMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        RoleMangDataAccess dbAccess = new RoleMangDataAccess();
        AuthManager auth = new AuthManager();

        private readonly IHostingEnvironment hostingEnvironment;

        public RoleMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlFunInfo = dbAccess.GetAllFunInfo();
            ViewBag.ddlLifeClass = dbAccess.GetAllLifeClass();

            RoleMangViewModel vm = new RoleMangViewModel();
            vm.ConditionModel = new RoleMangConditionModel();

            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlFunInfo = dbAccess.GetAllFunInfo2();

            RoleMangViewModel vm = new RoleMangViewModel();
            vm.CreateModel = new RoleMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, RoleMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlFunInfo = dbAccess.GetAllFunInfo2();

            vm.EditModel = dbAccess.GetEditData(submitBtn);
            vm.EditModel.LstFunItem = dbAccess.GetUserFunInfo(vm.EditModel.RoleId);

            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(RoleMangViewModel vm)
        {
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            List<FunInfo> LstAllFunInfo = new List<FunInfo>();
            auth.SelectAllFunInfo(null, out LstAllFunInfo);

            List<RoleFunInfo> LstAllRoleFun = new List<RoleFunInfo>();
            auth.SelectAllRoleFun(out LstAllRoleFun);

            List<RoleFunInfo> oRoleFunInfo = new List<RoleFunInfo>();

            foreach (RoleMangResultModel item in vm.ResultModel)
            {
                List<FunInfo> oFunInfo = new List<FunInfo>();

                oRoleFunInfo = LstAllRoleFun.Where(x => x.RoleID == item.RoleId).ToList();

                foreach (RoleFunInfo item2 in oRoleFunInfo)
                {
                    FunInfo fun = new FunInfo();
                    fun = LstAllFunInfo.Where(x => x.MenuNode == item2.MenuNode && x.FunName != "初始頁" && item2.RoleID == item.RoleId).FirstOrDefault();

                    if (fun != null)
                        oFunInfo.Add(fun);
                }

                item.LstFunInfo = oFunInfo;
            }

            if (vm.ConditionModel.MenuNode != null) {

                List<RoleMangResultModel> oResult = new List<RoleMangResultModel>();

                foreach (RoleMangResultModel item in vm.ResultModel) {
                    var r = item.LstFunInfo.Where(x => x.MenuNode == vm.ConditionModel.MenuNode).FirstOrDefault();

                    if (r != null)
                        oResult.Add(item);
                }

                vm.ResultModel = oResult;
            }
                
                #region 分頁
        vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }

        [Log(LogActionChineseName.新增儲存)]
        [ValidateInput(false)]
        public IActionResult SaveNewData(RoleMangViewModel vm)
        {
            try
            {
                vm.EditModel = dbAccess.GetEditData(vm.CreateModel.RoleId);

                if (vm.EditModel != null)
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗，角色代碼" + vm.CreateModel.RoleId + "已存在";
                    return Json(vmRtn);
                }

                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.InsertData(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                if (!string.IsNullOrEmpty(vm.CreateModel.strFunInfo))
                {

                    string[] arr = vm.CreateModel.strFunInfo.Split(",");

                    if (arr.Length > 0)
                    {
                        DataTable dt = dbAccess.GetUpMenuNode(arr);

                        foreach (DataRow dr in dt.Rows)
                        {
                            arr = arr.Concat(new string[] { dr["MenuUpNode"].ToString() }).ToArray();
                        }

                        arr = arr.Concat(new string[] { "202303180000001" }).ToArray();

                        dbResult = dbAccess.UpdateFunData(vm, arr);

                        if (!dbResult.isSuccess)
                        {
                            dbAccess.DbaRollBack();
                            vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                            vmRtn.ErrorMsg = "修改失敗";
                            return Json(vmRtn);
                        }
                    }
                }

                dbAccess.DbaCommit();
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "新增失敗" + ex.Message;
                return Json(vmRtn);
            }

            return Json(vmRtn);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public IActionResult EditOldData(RoleMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdateData(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "修改失敗";
                    return Json(vmRtn);
                }


                if (!string.IsNullOrEmpty(vm.EditModel.strFunInfo))
                {

                    string[] arr = vm.EditModel.strFunInfo.Split(",");

                    if (arr.Length > 0)
                    {
                        DataTable dt = dbAccess.GetUpMenuNode(arr);

                        HashSet<string> existingItems = new HashSet<string>(arr);


                        foreach (DataRow dr in dt.Rows)
                        {
                            string menuUpNode = dr["MenuUpNode"].ToString();
                            // 檢查是否已存在
                            if (!existingItems.Contains(menuUpNode))
                            {
                                // 加入到HashSet和arr中
                                existingItems.Add(menuUpNode);
                                arr = arr.Concat(new string[] { menuUpNode }).ToArray();
                            }
                        }

                        //加入各系統初始頁
                        string[] arr2 = dbAccess.GetDefaultPage(arr);
                        arr = arr.Concat(arr2).ToArray();

                        dbResult = dbAccess.UpdateFunData(vm, arr);

                        if (!dbResult.isSuccess)
                        {
                            dbAccess.DbaRollBack();
                            vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                            vmRtn.ErrorMsg = "修改失敗";
                            return Json(vmRtn);
                        }
                    }
                }

                dbAccess.DbaCommit();
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "修改失敗" + ex.Message;
                return Json(vmRtn);
            }

            return Json(vmRtn);
        }

        [Log(LogActionChineseName.刪除)]
        [ValidateInput(false)]
        public IActionResult Delete(string Ser)
        {
            try
            {
                DataTable dt = dbAccess.ChkHasOtherUseRole(Ser);

                if (dt.Rows.Count > 0)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "此角色仍有帳號在使用，不可刪除";
                    return Json(vmRtn);
                }

                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.DeleteFunData(Ser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode =  (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "刪除失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.DeletetUserRole(Ser);

                if (!dbResult.isSuccess && dbResult.ErrorCode != dbErrorCode._EC_NotAffect)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "刪除失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.DeletetData(Ser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "刪除失敗";
                    return Json(vmRtn);
                }

                dbAccess.DbaCommit();
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "刪除失敗" + ex.Message;
                return Json(vmRtn);
            }

            return Json(vmRtn);
        }

        #region Method

        #endregion
    }
}
