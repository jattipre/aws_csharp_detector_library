using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OjasMart.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;

namespace OjasMart.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales
        PropertyClass objp = new PropertyClass();
        LogicClass objL = new LogicClass();
        PaytmPayment paid = new PaytmPayment();
        public ActionResult GenerateSalesBill()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            if (Session["Role"].ToString() == "1")
            {
                objp.UserName = Convert.ToString(Session["CompanyCode"]);
                // ViewBag.CustomerList = PropertyClass.BindDDL(objL.BindCustomerDropDown());
            }
            else
            {
                objp.UserName = Convert.ToString(Session["UserName"]);
                //ViewBag.CustomerList = PropertyClass.BindDDL(objL.BindCustomerDropDownNew(objp.UserName));
            }
            ViewBag.CustomerList = PropertyClass.BindDDL(objL.BindCustomerDropDown());
            ViewBag.StateList = PropertyClass.BindDDL(objL.BindStateDropDown());
            ViewBag.ItemGroupList = PropertyClass.BindDDL(objL.BindItemGroupDropDown());
            //ViewBag.ItemHeadList = PropertyClass.BindDDL(objL.BindItemheadDropDown());

            objp.dt1 = objL.BindItemheadDropDown1(Session["UserName"].ToString());

            objp.eDate = DateTime.Today.ToString("dd-MMM-yyyy");

            objp.Action = "1";
            objp.dt = objL.GetActiveOffers(objp, "Proc_GetActiveOffers");
            DataTable dt = objL.GetLogingCompanyStateCode(objp, "getLoginCompanyDetails");
            if (dt != null && dt.Rows.Count > 0)
            {
                objp.StateName = dt.Rows[0]["StateCode"].ToString();
            }
            return View(objp);
        }
        public JsonResult InsertCustomerAccounts(PropertyClass p)
        {
            string msg = "";
            try
            {
                p.Action = "1";
                p.CompanyCode = Convert.ToString(Session["CompanyCode"]);
                if (Session["Role"].ToString() == "1")
                {
                    p.SSCode = Convert.ToString(Session["CompanyCode"]);
                }
                else
                {
                    p.SSCode = Convert.ToString(Session["UserName"]);
                }
                DataTable dt = objL.InsertCustomerAccount(p, "Proc_InserCustomerAccount");
                if (dt != null && dt.Rows.Count > 0)
                {
                    msg = dt.Rows[0]["msg"].ToString();

                    //if (!string.IsNullOrEmpty(p.ContactNo))
                    //{
                    //    string msg1 = "Dear " + p.SSName + " Thankyou for joining us. Please find your Account Login Details Below. User Name: " + dt.Rows[0]["UserId"].ToString() + ". Password: " + dt.Rows[0]["Password"].ToString() + ". Thankyou Team OZAS Mart. http://ozas199.com .";

                    //    //objp.sendsms(p.ContactNo, msg1);
                    //}
                }
                else
                {
                    msg = dt.Rows[0]["msg"].ToString();
                }
            }
            catch (Exception ex)
            {
                msg = "0";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerAccDetail(string AccountCode)
        {
            DataTable dt = new DataTable();
            dt = objL.GetCustomerAccDetail(AccountCode);
            if (dt != null && dt.Rows.Count > 0)
            {
                objp.AccountCode = dt.Rows[0]["account_code"].ToString();
                objp.StCode = dt.Rows[0]["StateCode"].ToString();
                objp.AccountType = dt.Rows[0]["AccountType"].ToString();
                objp.WalletBal = dt.Rows[0]["WalletBal"].ToString();
                objp.ContactNo = dt.Rows[0]["MobileNo"].ToString();
                objp.CashBackBalance= dt.Rows[0]["CasbckAmt"].ToString();
            }
            return Json(objp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerAccDetaiByMobilel(string MobileNo)
        {
            DataTable dt = new DataTable();
            dt = objL.GetCustomerAccDetailByMobiles(MobileNo);
            if (dt != null && dt.Rows.Count > 0)
            {
                objp.ContactNo = dt.Rows[0]["MobileNo"].ToString();
                objp.CustomerId = dt.Rows[0]["CustomerId"].ToString();
                objp.SSName = dt.Rows[0]["Name"].ToString();
            }
            else
            {
                objp.ContactNo = "0";
            }
            return Json(objp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertSaleOrder(string CustomerAccCode, string InvoiceDate, string StateId, string DeliveryTo, string TermsCond, string notes, decimal DiscPer, decimal DiscAmt, decimal CgstAmt, decimal SgstAmt, decimal IgstAmt, decimal PayableAmt, decimal GrossPayable, decimal NetTotal, decimal Payablegst, decimal RoundOffAmt, string BillingAddress, string PayMode, string ChqNo, string ChqDate, string BankAccName, string BankTransId, decimal PaidAmount, string PurchaseBy, string IsCashbackapply, decimal cashbackamt, string OfferCode, decimal OfferAmt, List<ItemDetails> ItemList, List<AppliedOfferList> offerList)
        {
            string msg = ""; 
            try
            {
                objp.Action = "1";
                objp.SupplierAccCode = CustomerAccCode;
                objp.InvoiceDate = Convert.ToDateTime(InvoiceDate);
                objp.StateId = StateId != "" ? Convert.ToInt32(StateId) : 09;
                objp.DeliveryTo = DeliveryTo;
                objp.TermsCond = TermsCond;
                objp.notes = notes;
                objp.DiscPer = DiscPer;
                objp.DiscAmt = DiscAmt;
                objp.CgstAmt = IgstAmt / 2;
                objp.SgstAmt = IgstAmt / 2;
                objp.IgstAmt = IgstAmt;
                objp.PayableAmt = PayableAmt;
                objp.GrossPayable = GrossPayable;
                objp.NetTotal = NetTotal;
                objp.Payablegst = Payablegst;
                objp.RoundOffAmt = RoundOffAmt;
                objp.PayMode = PayMode;
                objp.ChqNo = ChqNo;
                objp.ChqDate = (!string.IsNullOrEmpty(ChqDate) ? Convert.ToDateTime(ChqDate) : Convert.ToDateTime("01/01/1900"));
                objp.BanKAccName = BankAccName;
                objp.BankTransId = BankTransId;
                objp.IsCashbackApplied = IsCashbackapply;
                objp.CashBackAmount = cashbackamt;
                objp.CompanyCode = Convert.ToString(Session["CompanyCode"]);
                if (Session["Role"].ToString() == "1")
                {
                    objp.BranchCode = Convert.ToString(Session["CompanyCode"]);
                }
                else
                {
                    objp.BranchCode = Convert.ToString(Session["UserName"]);
                }
                objp.EntryBy = Convert.ToString(Session["StaffCode"]);
                objp.PaidAmount = PaidAmount;
                objp.Address = BillingAddress;
                objp.PurchaseBy = PurchaseBy;
                objp.OfferCode = OfferCode;
                objp.OfferAmt = OfferAmt;

                DataTable dt = new DataTable();
                dt.Columns.Add("ItemCode");
                dt.Columns.Add("HSNCode");
                dt.Columns.Add("Quantity");
                dt.Columns.Add("Rate");
                dt.Columns.Add("DiscountPer");
                dt.Columns.Add("DiscountPer2");
                dt.Columns.Add("DiscountAmt");
                dt.Columns.Add("cgstPer");
                dt.Columns.Add("cgstAmt");
                dt.Columns.Add("sgstPer");
                dt.Columns.Add("sgstAmt");
                dt.Columns.Add("igstPer");
                dt.Columns.Add("igstAmt");
                dt.Columns.Add("TotalAmount");
                dt.Columns.Add("UOM");
                dt.Columns.Add("MRP");
                dt.Columns.Add("TaxableAmount");
                dt.Columns.Add("varId");
                dt.Columns.Add("OfferCode");

                foreach (var item in ItemList)
                {
                    dt.Rows.Add(item.ItemCode, item.HSNCode, item.Quantity, item.Rate, item.DiscountPer, item.DiscountPer2, item.DiscountAmt, item.cgstPer, item.cgstAmt, item.sgstPer, item.sgstAmt, item.igstPer, item.igstAmt, item.TotalAmount, item.UOM, item.MRP, item.TaxableAmount, item.Reason, item.OfferCode);
                }

                DataTable dtoffer = new DataTable();
                dtoffer.Columns.Add("SrNo");
                dtoffer.Columns.Add("OfferCode");
                if (offerList != null && offerList.Count > 0)
                {
                    int i = 1;
                    foreach (var itm in offerList)
                    {
                        dtoffer.Rows.Add(i, itm.OfferCode);
                        i++;
                    }

                }

                DataTable dt1 = objL.InsertSalesOrder(objp, "Proc_InsertSalesOrderTest1", dt, dtoffer);
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    msg = dt1.Rows[0]["InvNo"].ToString();
                }
            }
            catch (Exception ex)
            {
                msg = "0";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSalesBillDetails(string StartDate, string EndDate, string MobileNo, string BillBy)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyCode"])))
            {
                return RedirectToAction("Index", "Account");
            }
            ViewBag.SSList = PropertyClass.BindDDL(objL.BindStockiestDropDownNew());
            objp.Action = "1";
            objp.mDate = StartDate;
            objp.eDate = EndDate;
            objp.CompanyCode = Convert.ToString(Session["CompanyCode"].ToString());
            objp.UserName = (!string.IsNullOrEmpty(BillBy)) ? BillBy : null;
            objp.ContactNo = (!string.IsNullOrEmpty(MobileNo)) ? MobileNo : null;

            if (Convert.ToString(Session["Role"]) == "2" || Convert.ToString(Session["Role"]) == "3")
            {
                objp.UserName = Convert.ToString(Session["UserName"]);
            }
            if (Convert.ToString(Session["Role"]) == "4")
            {
                objp.CustomerId = Convert.ToString(Session["UserName"]);
            }
            if (Session["Role"].ToString() != "1" && Session["Role"].ToString() != "2" && Session["Role"].ToString() != "3" && Session["Role"].ToString() != "4")
            {
                objp.EntryBy = Convert.ToString(Session["StaffCode"]);
            }

            objp.Role = Convert.ToString(Session["Role"].ToString());
            objp.dt = objL.GetSalesBillDetails(objp, "Proc_SalesBillDetails");
            decimal totgst = 0, totalAmt = 0, grossAmt = 0;
            if (objp.dt != null && objp.dt.Rows.Count > 0)
            {
                foreach (DataRow dr in objp.dt.Rows)
                {
                    totgst += dr["Totaltax"].ToString() != "" ? Convert.ToDecimal(dr["Totaltax"].ToString()) : 0;
                    totalAmt += dr["NetTotal"].ToString() != "" ? Convert.ToDecimal(dr["NetTotal"].ToString()) : 0;
                    grossAmt += dr["GrossPayable"].ToString() != "" ? Convert.ToDecimal(dr["GrossPayable"].ToString()) : 0;
                }

                objp.Payablegst = totgst;
                objp.NetTotal = totalAmt;
                objp.GrossPayable = grossAmt;
            }
            return View(objp);



        }
        public JsonResult SaleBillItemWiseDetails(string InvoiceNo)
        {
            PropertyClass model = new PropertyClass();
            DataTable dt = new DataTable();
            string CompanyCode = Convert.ToString(Session["CompanyCode"]);
            objp.Action = "2";
            objp.InvoiceNo = InvoiceNo;
            dt = objL.GetSalesBillDetails(objp, "Proc_SalesBillDetails");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PropertyClass m = new PropertyClass();
                    m.ItemCode = dr["ItemCode"].ToString();
                    m.ItemName = dr["ItemName"].ToString();
                    m.HSNCode = dr["HSNCode"].ToString();
                    m.UOM = dr["Size"].ToString();
                    m.Quantity = dr["Quantity"].ToString() != "" ? Convert.ToDecimal(dr["Quantity"].ToString()) : 0;
                    m.Rate = dr["Rate"].ToString() != "" ? Convert.ToDecimal(dr["Rate"].ToString()) : 0;
                    m.DiscPer = dr["DiscountPer"].ToString() != "" ? Convert.ToDecimal(dr["DiscountPer"].ToString()) : 0;
                    m.DiscPer2 = dr["DiscountPer2"].ToString() != "" ? Convert.ToDecimal(dr["DiscountPer2"].ToString()) : 0;
                    m.DiscAmt = dr["DiscountAmount"].ToString() != "" ? Convert.ToDecimal(dr["DiscountAmount"].ToString()) : 0;
                    m.Payablegst = dr["TaxAmount"].ToString() != "" ? Convert.ToDecimal(dr["TaxAmount"].ToString()) : 0;
                    m.GrossPayable = dr["TaxableAmount"].ToString() != "" ? Convert.ToDecimal(dr["TaxableAmount"].ToString()) : 0;
                    m.NetTotal = dr["NetAmount"].ToString() != "" ? Convert.ToDecimal(dr["NetAmount"].ToString()) : 0;
                    model.poList.Add(m);
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckStockAvailibility(string ItemCode1)
        {
            string msg = "";
            PropertyClass model = new PropertyClass();
            DataTable dt = new DataTable();
            if (Session["Role"].ToString() == "1")
            {
                objp.CompanyCode = Convert.ToString(Session["CompanyCode"]);
            }
            else
            {
                objp.CompanyCode = Convert.ToString(Session["UserName"]);
            }
            objp.Action = "1";
            objp.ItemCode = ItemCode1;
            objp.Role = Convert.ToString(Session["Role"]);
            dt = objL.CheckStockBalance(objp, "Proc_GetStockBalance");
            if (dt != null && dt.Rows.Count > 0)
            {
               
                msg = dt.Rows[0]["TotalUnit"].ToString();   
            }
            else
            {
                msg = "0";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemsByGroup(string GroupCode)
        {

            string BrannchCode = Convert.ToString(Session["UserName"]);
            DataTable dt = objL.BindItemheadByGroupDropDown1(GroupCode, BrannchCode);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow ds in dt.Rows)
                {
                    objp.ItemList.Add(new SelectListItem { Text = Convert.ToString(ds["ItemName"]), Value = Convert.ToString(ds["ItemCode"]) });
                }
            }

            return Json(new SelectList(objp.ItemList, "Value", "Text"));
        }

        public JsonResult GetItemsByGroupForBranch(string GroupCode)
        {

            // string BrannchCode = Convert.ToString(Session["UserName"]);
            string BrannchCode = null;
            DataTable dt = objL.BindItemheadByGroupDropDown1(GroupCode, BrannchCode);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow ds in dt.Rows)
                {
                    objp.ItemList.Add(new SelectListItem { Text = Convert.ToString(ds["ItemName"]), Value = Convert.ToString(ds["ItemCode"]) });
                }
            }

            return Json(new SelectList(objp.ItemList, "Value", "Text"));
        }

        public JsonResult InsertWalletRecharge(PropertyClass p)
        {
            try
            {
                string msg = "";
                DataTable dt = new DataTable();
                p.CompanyCode = Convert.ToString(Session["CompanyCode"]);
                if (Session["Role"].ToString() == "1")
                {
                    p.SSCode = Convert.ToString(Session["CompanyCode"]);
                }
                else
                {
                    p.SSCode = Convert.ToString(Session["UserName"]);
                }
                p.Action = "1";
                p.EntryBy = Convert.ToString(Session["StaffCode"]);
                dt = objL.InsertWalletRecharge(p, "Proc_WalletRecharge");
                if (dt != null && dt.Rows.Count > 0)
                {
                    objp.msg = dt.Rows[0]["id"].ToString();
                    objp.WalletBalance = dt.Rows[0]["WalletAmt"].ToString();
                    objp.txnId = dt.Rows[0]["transactionId"].ToString();
                    string CustName = dt.Rows[0]["CustName"].ToString();
                    string RechrgDate = dt.Rows[0]["RechrgDate"].ToString();

                    string MobNo = dt.Rows[0]["MobNo"].ToString();
                    if (MobNo.Trim() != "")
                    {
                        string msg1 = "Dear " + CustName + " Your Wallet Credited with Rs." + p.PaidAmount + "," + RechrgDate + " Your Wallet Balance is Rs." + objp.WalletBalance + ".";
                        objp.SendSMS(MobNo.Trim(), msg1);
                    }
                }
                else
                {
                    objp.msg = "0";
                }
            }
            catch (Exception ex)
            {

            }
            return Json(objp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenerateAndSendOTP(string Customerid)
        {
            string otpmsg = "";
            Random generator = new Random();
            String r = generator.Next(0, 999999).ToString("D6");
            string MobileNo = "";

           
            if (!string.IsNullOrEmpty(Customerid))
            {
                MobileNo = Customerid.Trim();
                if (MobileNo == "")
                {
                    otpmsg = "0";
                }
                else
                {
                    otpmsg = r;
                    string msg = "One Time Password For Mobile Verification is: " + r;
                    objp.SendSMS(MobileNo, msg);
                }
            }
            else
            {
                otpmsg = "0";
            }
            //divOtpSec.Visible = true;

            return Json(otpmsg, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public JsonResult GetItemDetailByBarCode(string barCode,string VarId, string MRP, int Action,string ItemCode)
        {
            decimal C_Mrp = 0;
            decimal.TryParse(Convert.ToString(MRP),out C_Mrp); 
            DataTable dt = new DataTable();
            try
            {
                dt = objL.FetchItemByBarCode(barCode, VarId, C_Mrp, Action,ItemCode);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int id = 0;
                    int.TryParse(Convert.ToString(dt.Rows[0]["Id"]), out id);
                    

                    if (id > 0)
                    {
                        if (Action != 2)
                        {
                            decimal Rate = 0, AMRP = 0, GSTPer = 0;
                            decimal.TryParse(Convert.ToString(dt.Rows[0]["PurchaseRate"]), out Rate);
                            decimal.TryParse(Convert.ToString(dt.Rows[0]["SalePrice"]), out AMRP);
                            decimal.TryParse(Convert.ToString(dt.Rows[0]["GSTPer"]), out GSTPer);

                            objp.GroupCode = dt.Rows[0]["ItemGroup"].ToString();
                            objp.ItemCode = dt.Rows[0]["ItemCode"].ToString();
                            objp.HSNCode = dt.Rows[0]["HSNCode"].ToString();
                            objp.UOM = dt.Rows[0]["VarriationName"].ToString();
                            objp.VariationId = dt.Rows[0]["VariationId"].ToString();
                            objp.ItemID = dt.Rows[0]["ItmId"].ToString();

                            objp.Manufacturerid = dt.Rows[0]["Manufacturerid"].ToString();
                            objp.MainCategoryCode = dt.Rows[0]["ItemGroup"].ToString();


                            objp.Quantity = 1;
                            objp.Rate = Rate;
                            objp.MRP = AMRP;
                            objp.GSTPer = GSTPer;
                            objp.Id = 1;
                        }
                        else {
                            objp.Id = 1;
                        }

                    }
                    else
                    {
                        objp.Id = 0;
                        objp.msg = Convert.ToString(dt.Rows[0]["msg"]);
                    }
                    
                }
                else
                {
                    objp.Id = 0;
                    objp.msg = "record not found";
                }
            }
            catch (Exception ex)
            {
                objp.Id = 0;
                objp.msg = "something went wrong!!";
            }
            return Json(objp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendSMS()
        {
          //  objp.SendSMS("7355644826", "Hello");
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public void InsertMethod()
        {
            try
            {
                string TRID = "";
                string txnid1 = "";
                if (string.IsNullOrEmpty(Request.Form["txnid"])) // generating txnid
                {
                    Random rnd = new Random();
                    TRID = rnd.ToString() + DateTime.Now;
                    string strHash = Generatehash512(TRID);
                    txnid1 = strHash.ToString().Substring(0, 20).ToUpper();
                }
                else
                {
                    txnid1 = Request.Form["txnid"];
                }
                BookingRequest("1251155012", "1000210", "100", "Janu Hassan", "abc@gmail.com", "7355644826", "Item", "../Sales/sUrl", "../Sales/sUrl", txnid1);
            }
            catch
            {
            }

        }
        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }
        public void BookingRequest(string TID, string BookingID, string Amount, string Name, string email, string phone, string productinfo, string surl, string furl, string txnid2)
        {
            try
            {
                paid.EpaytmPayment(TID, BookingID, Convert.ToDecimal(Amount), phone, email);
            }

            catch (Exception ex)
            {
                Response.Write("<span style='color:red'>" + ex.Message + "</span>");

            }

        }

        public ActionResult sUrl()
        {
            return View();
        }

        public ActionResult paytmcheckout()
        {
            string MID = "LhCnEZ84716866917246";
            string REQUEST_TYPE = "DEFAULT";
            string CHANNEL_ID = "WEB";
            string INDUSTRY_TYPE_ID = "Retail";
            string WEBSITE = "DEFAULT";

            String merchantKey = "sV&Z_3cTWQLw7QIL";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("MID", MID);
            parameters.Add("CHANNEL_ID", CHANNEL_ID);
            parameters.Add("INDUSTRY_TYPE_ID", INDUSTRY_TYPE_ID);
            parameters.Add("WEBSITE", WEBSITE);
            parameters.Add("EMAIL", "abc@gmail.com");
            parameters.Add("MOBILE_NO", "7355644826");
            parameters.Add("CUST_ID", "123456");
            parameters.Add("ORDER_ID", "1001");
            parameters.Add("TXN_AMOUNT", "100");
            parameters.Add("CALLBACK_URL", "http://localhost:9349/Sales/sUrl");
            //This parameter is not mandatory. Use this to pass the callback url dynamically.

            string checksum = paytm.CheckSum.generateCheckSum(merchantKey, parameters);

            string paytmURL = "https://pguat.paytm.com/oltp-web/processTransaction";

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            foreach (string key in parameters.Keys)
            {
                outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
            }
            outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";
            Response.Write(outputHTML);
            //return new EmptyResult();
            return View();
        }
        public JsonResult CHeckAddOnMembers(string Customerid)
        {
            string st = "";

            DataTable dt = objL.CheckAddOnMebers(Customerid);
            if (dt != null && dt.Rows.Count > 0)
            {
                st = "1";
            }
            else
            {
                st = "0";
            }
            return Json(st, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CHeckCashBackAmtBalance(string Customerid)
        {
            string CsbAmt = "";

            DataTable dt = objL.CHeckCashBackAmtBalance(Customerid);
            if (dt != null && dt.Rows.Count > 0)
            {
                CsbAmt = dt.Rows[0]["CashbackAmt"].ToString();
            }
            else
            {
                CsbAmt = "0";
            }
            return Json(CsbAmt, JsonRequestBehavior.AllowGet);
        }

        #region Sale Return

        public ActionResult GenerateSaleReturn()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            
            return View();

        }

        public JsonResult GetInvNo()
        {
            clsSaleReturn objBase = new clsSaleReturn();
            try
            {
                DataTable dt = objL.GetInvSearch(1, null);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        objBase.InvList.Add(new SelectListItem { Text = Convert.ToString(dr["InvoiceNo"]), Value = Convert.ToString(dr["InvoiceNo"]) });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(new SelectList(objBase.InvList, "Value", "Text"));
        }

        public JsonResult GetInvoiceDetails(string InvNo)
        {
            clsSaleReturn objR = new clsSaleReturn();
            try
            {
                DataTable ds = new DataTable();
                ds = objL.GetInvSearch(2, InvNo);
                if (ds != null && ds.Rows.Count > 0)
                {
                    objR.msg = "1";

                    objR.MobileNo = Convert.ToString(ds.Rows[0]["MobileNo"]);
                    objR.CustomerName = Convert.ToString(ds.Rows[0]["CustomerName"]);
                    objR.CustomerAccountCode = Convert.ToString(ds.Rows[0]["CustomerAccountCode"]);
                    objR.VoucherId = Convert.ToString(ds.Rows[0]["VoucherId"]);
                    objR.InvoiceDate = Convert.ToString(ds.Rows[0]["InvoiceDate"]);
                    objR.BillingAddress = Convert.ToString(ds.Rows[0]["BillingAddress"]);
                    objR.GrossPayable = Convert.ToString(ds.Rows[0]["GrossPayable"]);
                    objR.Totaltax = Convert.ToString(ds.Rows[0]["Totaltax"]);
                    objR.NetTotal = Convert.ToString(ds.Rows[0]["NetTotal"]);
                    objR.DiscountAmount = Convert.ToString(ds.Rows[0]["DiscountAmount"]);
                    objR.PaymentMode = Convert.ToString(ds.Rows[0]["PaymentMode"]);


                    DataTable dss = new DataTable();
                    dss = objL.GetInvSearch(3, InvNo);
                    if (dss != null && dss.Rows.Count > 0)
                    {
                        objR.msg = "1";

                        objR.InvDetails = ConvertDataTabletoString(dss);

                    }
                    else {
                        objR.msg = "0";
                    }





                    }
                else {
                    objR.msg = "0";
                }
            }
            catch (Exception ex)
            {
                objR.msg = "0";
            }
            return Json(objR, JsonRequestBehavior.AllowGet);
        }

        public static string ConvertDataTabletoString(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        public JsonResult InsertSaleOrderReturn(string CustomerAccCode, string InvoiceNo, string VoucherNo
            ,decimal GrossPayable,  decimal Payablegst, decimal NetTotal,
            List<ItemDetails> ItemList)
        {
            string msg = "";
            try
            {
                objp.Action = "1";
                objp.SupplierAccCode = CustomerAccCode;
                objp.InvoiceNo = InvoiceNo;
                objp.VoucherId = VoucherNo;
                objp.GrossPayable = GrossPayable;
                objp.Payablegst = Payablegst;
                objp.NetTotal = NetTotal;

                objp.CompanyCode = Convert.ToString(Session["CompanyCode"]);
                if (Session["Role"].ToString() == "1")
                {
                    objp.BranchCode = Convert.ToString(Session["CompanyCode"]);
                }
                else
                {
                    objp.BranchCode = Convert.ToString(Session["UserName"]);
                }
                objp.EntryBy = Convert.ToString(Session["StaffCode"]);

                
                DataTable dt = new DataTable();
                dt.Columns.Add("ItemCode");
                dt.Columns.Add("Quantity");
                dt.Columns.Add("VarId");
                foreach (var item in ItemList)
                {
                    dt.Rows.Add(item.ItemCode, item.Quantity,item.VariationId);
                }
                
                DataTable dt1 = objL.InsertSalesOrderReturn(objp, "Proc_InsertSalesOrderReturn", dt);
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    msg = dt1.Rows[0]["InvNo"].ToString();
                }
                else {
                    msg = "0";
                }


            }
            catch (Exception ex)
            {
                msg = "0";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }







        #endregion

        #region Sale Return Listing

        public ActionResult rptsalereturn(string StartDate, string EndDate, string MobileNo)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyCode"])))
            {
                return RedirectToAction("Index", "Account");
            }
            ViewBag.SSList = PropertyClass.BindDDL(objL.BindStockiestDropDownNew());
            objp.Action = "4";
            objp.mDate = StartDate;
            objp.eDate = EndDate;
            objp.CompanyCode = Convert.ToString(Session["CompanyCode"].ToString());
          //  objp.UserName = (!string.IsNullOrEmpty(BillBy)) ? BillBy : null;
            objp.ContactNo = (!string.IsNullOrEmpty(MobileNo)) ? MobileNo : null;

            if (Convert.ToString(Session["Role"]) == "2" || Convert.ToString(Session["Role"]) == "3")
            {
                objp.UserName = Convert.ToString(Session["UserName"]);
            }
            if (Convert.ToString(Session["Role"]) == "4")
            {
                objp.CustomerId = Convert.ToString(Session["UserName"]);
            }
            if (Session["Role"].ToString() != "1" && Session["Role"].ToString() != "2" && Session["Role"].ToString() != "3" && Session["Role"].ToString() != "4")
            {
                objp.EntryBy = Convert.ToString(Session["StaffCode"]);
            }

            objp.Role = Convert.ToString(Session["Role"].ToString());
            objp.dt = objL.GetSalesBillDetails(objp, "Proc_SalesBillDetails");
            decimal totgst = 0, totalAmt = 0, grossAmt = 0;
            if (objp.dt != null && objp.dt.Rows.Count > 0)
            {
                foreach (DataRow dr in objp.dt.Rows)
                {
                    totgst += dr["Totaltax"].ToString() != "" ? Convert.ToDecimal(dr["Totaltax"].ToString()) : 0;
                    totalAmt += dr["NetTotal"].ToString() != "" ? Convert.ToDecimal(dr["NetTotal"].ToString()) : 0;
                    grossAmt += dr["GrossPayable"].ToString() != "" ? Convert.ToDecimal(dr["GrossPayable"].ToString()) : 0;
                }

                objp.Payablegst = totgst;
                objp.NetTotal = totalAmt;
                objp.GrossPayable = grossAmt;
            }
            return View(objp);



        }

        public JsonResult SaleBillReturnItemWiseDetails(string InvoiceNo)
        {
            PropertyClass model = new PropertyClass();
            DataTable dt = new DataTable();
            string CompanyCode = Convert.ToString(Session["CompanyCode"]);
            objp.Action = "5";
            objp.InvoiceNo = InvoiceNo;
            dt = objL.GetSalesBillDetails(objp, "Proc_SalesBillDetails");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PropertyClass m = new PropertyClass();
                    m.ItemCode = dr["ItemCode"].ToString();
                    m.ItemName = dr["ItemName"].ToString();
                    m.HSNCode = dr["HSNCode"].ToString();
                    m.UOM = dr["Size"].ToString();
                    m.Quantity = dr["Quantity"].ToString() != "" ? Convert.ToDecimal(dr["Quantity"].ToString()) : 0;
                    m.Rate = dr["Rate"].ToString() != "" ? Convert.ToDecimal(dr["Rate"].ToString()) : 0;
                    m.DiscPer = dr["DiscountPer"].ToString() != "" ? Convert.ToDecimal(dr["DiscountPer"].ToString()) : 0;
                    m.DiscPer2 = dr["DiscountPer2"].ToString() != "" ? Convert.ToDecimal(dr["DiscountPer2"].ToString()) : 0;
                    m.DiscAmt = dr["DiscountAmount"].ToString() != "" ? Convert.ToDecimal(dr["DiscountAmount"].ToString()) : 0;
                    m.Payablegst = dr["TaxAmount"].ToString() != "" ? Convert.ToDecimal(dr["TaxAmount"].ToString()) : 0;
                    m.GrossPayable = dr["TaxableAmount"].ToString() != "" ? Convert.ToDecimal(dr["TaxableAmount"].ToString()) : 0;
                    m.NetTotal = dr["NetAmount"].ToString() != "" ? Convert.ToDecimal(dr["NetAmount"].ToString()) : 0;
                    model.poList.Add(m);
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }



        #endregion


    }
}