using Installments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;


namespace Installments.Controllers
{
    public class SaleInfoController : Controller
    {
        
        // GET: SaleInfo
        public ActionResult Index()
        {
            string SQL = @"
select SaleInfo.SaleID,SaleInfo.SaleDate,SaleInfo.CustomerName,SaleInfo.CustomerCNIC,SaleInfo.CustomerAddress,SaleInfo.CustomerCity
,SaleInfo.CustomerMobileNo,SaleInfo.CustomerMobileNo2,SaleInfo.BillingAddress,
SaleInfo.SaleNo,SaleInfo.Status,(Select Sum(Price*Quantity) From SaleDetail 
where SaleDetail.SaleID= SaleInfo.SaleID ) as TotalCost from SaleInfo";
            DataTable dt = General.FetchData(SQL);
            return View(dt);        
        }
        public ActionResult Getorder() { 
            string SQL = @"
select SaleInfo.SaleID,SaleInfo.SaleDate,SaleInfo.CustomerName,SaleInfo.CustomerCNIC,SaleInfo.CustomerAddress,SaleInfo.CustomerCity
,SaleInfo.CustomerMobileNo,SaleInfo.CustomerMobileNo2,SaleInfo.BillingAddress,
SaleInfo.SaleNo,SaleInfo.Status,(Select Sum(Price*Quantity) From SaleDetail 
where SaleDetail.SaleID= SaleInfo.SaleID ) as TotalCost from SaleInfo where status=0";
        DataTable dt = General.FetchData(SQL);
            if (dt.Rows.Count > 0)
            {
                ViewBag.Status = dt.Rows[0]["Status"].ToString();
            }
            return View("Index", dt);
        }
        public ActionResult GetConfirmed()
        {
            string SQL = @"
select SaleInfo.SaleID,SaleInfo.SaleDate,SaleInfo.CustomerName,SaleInfo.CustomerCNIC,SaleInfo.CustomerAddress,SaleInfo.CustomerCity
,SaleInfo.CustomerMobileNo,SaleInfo.CustomerMobileNo2,SaleInfo.BillingAddress,
SaleInfo.SaleNo,SaleInfo.Status,(Select Sum(Price*Quantity) From SaleDetail 
where SaleDetail.SaleID= SaleInfo.SaleID ) as TotalCost from SaleInfo where status=1";
            DataTable dt = General.FetchData(SQL);
            if (dt.Rows.Count > 0)
            {
                ViewBag.Status = dt.Rows[0]["Status"].ToString();
            }
                return View("Index",dt);
        }
        public ActionResult GetDispatch()
        {
            string SQL = @"
select SaleInfo.SaleID,SaleInfo.SaleDate,SaleInfo.CustomerName,SaleInfo.CustomerCNIC,SaleInfo.CustomerAddress,SaleInfo.CustomerCity
,SaleInfo.CustomerMobileNo,SaleInfo.CustomerMobileNo2,SaleInfo.BillingAddress,
SaleInfo.SaleNo,SaleInfo.Status,(Select Sum(Price*Quantity) From SaleDetail 
where SaleDetail.SaleID= SaleInfo.SaleID ) as TotalCost from SaleInfo where status=2";
            DataTable dt = General.FetchData(SQL);
            if (dt.Rows.Count > 0)
            {
                ViewBag.Status = dt.Rows[0]["Status"].ToString();
            }
                return View("Index", dt);
        }
        public ActionResult GetDelivered()
        {
            string SQL = @"
select SaleInfo.SaleID,SaleInfo.SaleDate,SaleInfo.CustomerName,SaleInfo.CustomerCNIC,SaleInfo.CustomerAddress,SaleInfo.CustomerCity
,SaleInfo.CustomerMobileNo,SaleInfo.CustomerMobileNo2,SaleInfo.BillingAddress,
SaleInfo.SaleNo,SaleInfo.Status,(Select Sum(Price*Quantity) From SaleDetail 
where SaleDetail.SaleID= SaleInfo.SaleID ) as TotalCost from SaleInfo where status=3";
            DataTable dt = General.FetchData(SQL);
            if (dt.Rows.Count > 0)
            {
                ViewBag.Status = dt.Rows[0]["Status"].ToString();
            }
            return View("Index", dt);
        }
        public ActionResult CashReceived()
        {
            string SQL = @"
select SaleInfo.SaleID,SaleInfo.SaleDate,SaleInfo.CustomerName,SaleInfo.CustomerCNIC,SaleInfo.CustomerAddress,SaleInfo.CustomerCity
,SaleInfo.CustomerMobileNo,SaleInfo.CustomerMobileNo2,SaleInfo.BillingAddress,
SaleInfo.SaleNo,SaleInfo.Status,(Select Sum(Price*Quantity) From SaleDetail 
where SaleDetail.SaleID= SaleInfo.SaleID ) as TotalCost from SaleInfo where status=3";
            DataTable dt = General.FetchData(SQL);
            if (dt.Rows.Count > 0)
            {
                ViewBag.Status = dt.Rows[0]["Status"].ToString();
            }
                return View("Index", dt);
        }
        [HttpGet]
        public ActionResult ShowAllRecords()
        {
            string SQL = @"select * from saleinfo inner join saledetail on saleinfo.saleid= saledetail.SaleID left outer join ProductInfo on SaleDetail.ProductID=
ProductInfo.ProductID";
            DataTable dt = General.FetchData(SQL);
            return View(dt);
        }
        public ActionResult Create()
        {
            ViewBag.lstProduct= new DropDown().GetProductInfo();
            return View();
        }



        [HttpPost]
        // POST: SaleInfo/Create
        public JsonResult Create(SaleInfo sale, List<SaleDetail> lstSaleDetail)
        {
            try
            {
                if (lstSaleDetail == null)
                {
                    lstSaleDetail = new List<SaleDetail>();
                }
                DateTime PakistanStandardTime()
                {
                    DateTime date1 = DateTime.UtcNow;

                    TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");

                    DateTime date2 = TimeZoneInfo.ConvertTime(date1, tz);
                    return date2;
                }
                string sql = "select 'Sa' + '-' + Cast((count(*) + 1) as Nvarchar(10)) + '-' + CAST(YEAR(GetDate()) as nvarchar(50)) as SaleNo from SaleInfo";
                DataTable dt = General.FetchData(sql);
                sale.SaleNo = dt.Rows[0]["SaleNo"].ToString();
                string Query = @"Insert into SaleInfo(SaleDate,CustomerName,CustomerCNIC,CustomerAddress,CustomerCity,CustomerMobileNo,CustomerMobileNo2,SaleNo,TotalCost,BillingAddress,Status)";
                Query = Query + "Values ('" + PakistanStandardTime() + "','" + sale.CustomerName + "','" + sale.CustomerCNIC + "','" + sale.CustomerAddress + "','" + sale.CustomerCity + "','" + sale.CustomerMobileNO + "','" + sale.CustomerMobileNo2 + "','" + sale.SaleNo + "','" + sale.TotalCost + "','" + sale.BillingAddress + "','" + sale.Status + "')";
                Query = Query + " Select @@IDENTITY as SaleID";
                sale.SaleID = int.Parse(General.FetchData(Query).Rows[0]["SaleID"].ToString());
                Query = "";
                foreach (SaleDetail ass in lstSaleDetail)
                {
                    Query = Query + " Insert into SaleDetail(SaleID,ProductID,Price,Quantity,Discount,Remarks) Values('" + sale.SaleID + "','" + ass.ProductID + "','" + ass.Price + "','" + ass.Quantity + "','" + ass.Discount + "','" + ass.Remarks + "')";
                }
                if (Query != "")
                {
                    General.ExecuteNonQuery(Query);
                }
                DataTable dtSaleInfo = General.FetchData(@"Select SaleNo,CustomerName,CustomerCNIC,CustomerMobileNo,CustomerAddress,BillingAddress,(Select Sum(Price*Quantity) From SaleDetail 
where SaleDetail.SaleID = SaleInfo.SaleID) as TotalCost  from SaleInfo where SaleID = " + sale.SaleID);
                DataTable dtSaleDetail = General.FetchData(@"Select ProductInfo.Productcode,ProductInfo.ProductName,SaleDetail.Price,SaleDetail.Quantity,(SaleDetail.Quantity*SaleDetail.Price) as Amount from SaleDetail inner join ProductInfo on SaleDetail.ProductId = ProductInfo.ProductID  Where SaleID = " + sale.SaleID);
                Dictionary<string, object> Sales = GetTableFirstRow(dtSaleInfo);
                List<Dictionary<string, object>> details = GetTableRows(dtSaleDetail);
                Sales.Add("Details", details);
                return new JsonResult()
                {
                    Data = new
                    {
                        Sales,
                    },  
                    ContentType = "application/json",
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = Int32.MaxValue
                };
                //return Json(true);
            }
            catch(Exception)
            {
                return Json(false);
            }
        }
        [HttpGet]
//        public ActionResult GetSaleInfo()
//        {
//            DataTable dtsaleInfo = General.FetchData("Select *  from SaleInfo");
////            string sql = @"select SaleInfo.SaleNo,SaleInfo.SaleDate,SaleInfo.CustomerName,SaleInfo.CustomerCNIC,SaleInfo.CustomerAddress,SaleInfo.CustomerCity
////,SaleInfo.CustomerMobileNo,SaleInfo.CustomerMobileNo2,
////SaleInfo.BillingAddress,SaleInfo.Status,(Select Sum(Price*Quantity) From SaleDetail where SaleDetail.SaleID= SaleInfo.SaleID ) 
////as TotalCost from SaleInfo";
//            List<Dictionary<string, object>> dbrows = GetTableRows(dtsaleInfo);
//            Dictionary<string, object> JSResponse = new Dictionary<string, object>();
//            if(JSResponse==null)
//            {
//                JSResponse.Add("Status", false);
//            }
//            else
//            {
//                JSResponse.Add("Status", true);

//            }
//            JSResponse.Add("Message", " Data for All Sales ");
//            JSResponse.Add("Data", dbrows);
//            JsonResult jr = new JsonResult()
//            {
//                Data = JSResponse,
//                ContentType = "application/json",
//                ContentEncoding = System.Text.Encoding.UTF8,
//                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
//                MaxJsonLength = Int32.MaxValue
//            };
//            return jr;
//        }

        
        public JsonResult GetSalesdetailforapi(int saleid)
        {
            DataTable dtSaleInfo = General.FetchData(@"Select SaleNo,CustomerName,CustomerCNIC,CustomerMobileNo,CustomerAddress,BillingAddress,(Select Sum(Price*Quantity) From SaleDetail 
where SaleDetail.SaleID= SaleInfo.SaleID ) as TotalCost  from SaleInfo where SaleID = " + saleid);
            DataTable dtSaleDetail = General.FetchData(@"Select ProductInfo.Productcode,ProductInfo.ProductName,SaleDetail.Price,SaleDetail.Quantity,(SaleDetail.Quantity*SaleDetail.Price) as Amount from SaleDetail inner join ProductInfo on SaleDetail.ProductId = ProductInfo.ProductID  Where SaleID = " + saleid);
            Dictionary<string, object> dbrows = GetTableFirstRow(dtSaleInfo);
            List<Dictionary<string,object>> details = GetTableRows(dtSaleDetail);
            dbrows.Add("Details", details);
            return new JsonResult()
            {
                Data = new
                {
                    dbrows,

                },
                ContentType = "application/json",
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        [ValidateInput(false)]
        public List<Dictionary<string, object>> GetTableRows(DataTable dtData)
        {
            List<Dictionary<string, object>>
            lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;
            foreach (DataRow dr in dtData.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in dtData.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }
            return lstRows;
        }
        [ValidateInput(false)]
        public Dictionary<string, object> GetTableFirstRow(DataTable dtData)
        {
          
            Dictionary<string, object> dictRow = null;
            for(int i=0;i<1;i++)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in dtData.Columns)
                {
                    dictRow.Add(col.ColumnName, dtData.Rows[i][col]);
                }
                
            }
            return dictRow;
        }
        public List<Dictionary<string, object>> GetSaleDetail(DataTable dtData)
        {
            List<Dictionary<string, object>>
            lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in dtData.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in dtData.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }
            return lstRows;
        }

        // GET: SaleInfo/Edit/5
        public ActionResult Edit(int id)
        {
            
            //ViewBag.Delivery = new DropDown().GetDeliveryDetail();
            //ViewBag.ddlDel = new DropDown().GetDDlDel();
            //ViewBag.lstProduct = new DropDown().GetProductInfo();
            string Query = @"select SaleInfo.SaleID,SaleInfo.SaleDate,SaleInfo.CustomerName,SaleInfo.CustomerCNIC,SaleInfo.CustomerAddress,SaleInfo.CustomerCity
,SaleInfo.CustomerMobileNo,SaleInfo.CustomerMobileNo2,SaleInfo.TransportService,SaleInfo.BultiyNo,SaleInfo.CashReceived,
SaleInfo.BillingAddress,SaleInfo.SaleNo,SaleInfo.Status,(Select Sum(Price*Quantity) From SaleDetail where SaleDetail.SaleID= SaleInfo.SaleID ) 
as TotalCost from SaleInfo where SaleID=" + id;
            DataTable dt = General.FetchData(Query);
            SaleInfo objVoucher = new SaleInfo();
            SaleDetail objdetail = new SaleDetail();
            if (dt.Rows.Count > 0)
            {
                objVoucher.SaleID = int.Parse(dt.Rows[0]["SaleID"].ToString());
                objVoucher.SaleNo = dt.Rows[0]["SaleNo"].ToString();
                objVoucher.SaleDate = DateTime.Parse(dt.Rows[0]["SaleDate"].ToString());
                objVoucher.TotalCost = decimal.Parse(dt.Rows[0]["TotalCost"].ToString());
                objVoucher.CustomerName = dt.Rows[0]["CustomerName"].ToString();
                objVoucher.CustomerMobileNO = dt.Rows[0]["CustomerMobileNo"].ToString();
                objVoucher.CustomerMobileNo2 = dt.Rows[0]["CustomerMobileNo2"].ToString();
                objVoucher.CustomerCNIC = dt.Rows[0]["CustomerCNIC"].ToString();
                objVoucher.CustomerCity = dt.Rows[0]["CustomerCity"].ToString();
                objVoucher.CustomerAddress = (dt.Rows[0]["CustomerAddress"].ToString());
                objVoucher.BillingAddress = dt.Rows[0]["BillingAddress"].ToString();
                objVoucher.Status = int.Parse(dt.Rows[0]["Status"].ToString());
                if(dt.Rows[0]["CashReceived"]!=DBNull.Value)
                {
                    objVoucher.CashReceived = decimal.Parse(dt.Rows[0]["CashReceived"].ToString());
                }
                objVoucher.TransportService = dt.Rows[0]["TransportService"].ToString();
                objVoucher.BultiyNo = dt.Rows[0]["BultiyNo"].ToString();
                DataTable dtDetail = General.FetchData(@"Select SaleDetail.*,ProductInfo.ProductName,ProductInfo.Productcode from SaleDetail inner join ProductInfo on SaleDetail.ProductId=ProductInfo.ProductID
Where SaleID = " + id);
                if (dtDetail.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDetail.Rows)
                    {
                        objdetail = new SaleDetail();
                        objdetail.ProductID = int.Parse(dr["ProductID"].ToString());
                        objdetail.ProductTitle = (dr["ProductName"].ToString());
                        objdetail.Quantity = int.Parse(dr["Quantity"].ToString());
                        objdetail.SaleDetailID = int.Parse(dr["SaleDetailId"].ToString());
                        objdetail.SaleID = int.Parse(dr["SaleId"].ToString());
                        objdetail.Price = decimal.Parse(dr["Price"].ToString());
                        if (dr["Discount"] != DBNull.Value)
                        {
                            objdetail.Discount = decimal.Parse(dr["Discount"].ToString());
                        }
                        else
                        {
                            objdetail.Discount = 0;
                        }

                        objVoucher.lstSaleDetail.Add(objdetail);

                    }
                    var shortdetail = from e in objVoucher.lstSaleDetail
                                      group e by new { e.ProductID, e.Price, e.Discount, e.ProductTitle } into eg
                                      select new { eg.Key.ProductID, eg.Key.Price, eg.Key.Discount, eg.Key.ProductTitle, Qty = eg.Sum(rl => rl.Quantity) };

                    foreach (var o in shortdetail)
                    {
                        objdetail = new SaleDetail();
                        objdetail.ProductID = o.ProductID;
                        objdetail.Price = o.Price;
                        objdetail.Discount = o.Discount;
                        objdetail.Quantity = o.Qty;
                        objdetail.ProductTitle = o.ProductTitle;
                        objVoucher.lstShortSaleDetail.Add(objdetail);
                    }
                }
            }
            ViewBag.Delivery = new DropDown().GetDeliveryDetail(objVoucher.Status);
            ViewBag.Status = objVoucher.Status;
            if (objVoucher.Status==3)
            {
                ViewBag.ddlDel = new DropDown().GetDDlDel(1);
            }
            else
            {
                ViewBag.ddlDel = new DropDown().GetDDlDel();
            }
            ViewBag.lstProduct = new DropDown().GetProductInfo();
            return View(objVoucher);
        }


        // POST: SaleInfo/Edit/5
        [HttpPost]
        public JsonResult Edit(SaleInfo objsales, List<SaleDetail> lstSaleDetail)
        {
            try
            {
                string Query = @" Update SaleInfo set SaleDate='"+objsales.SaleDate+"'";
                Query = Query + " , CustomerName ='" + objsales.CustomerName + "'";
                Query = Query + " , CustomerCNIC = '" + objsales.CustomerCNIC + "'";
                Query = Query + " , CustomerAddress ='" + objsales.CustomerAddress + "'";
                Query = Query + " , CustomerCity ='" + objsales.CustomerCity + "'";
                Query = Query + " , CustomerMobileNo ='" + objsales.CustomerMobileNO + "'";
                Query = Query + " , CustomerMobileNo2 ='" + objsales.CustomerMobileNo2 + "'";
                Query = Query + " , SaleNo ='" + objsales.SaleNo + "'";
                Query = Query + " , BillingAddress ='" + objsales.BillingAddress + "'";
                Query = Query + " , TotalCost ='" + objsales.TotalCost + "'";
                Query = Query + " , Status ='" + objsales.Status + "'";
                Query = Query + ",TransportService='" + objsales.TransportService + "'";
                Query = Query + ",BultiyNo='" + objsales.BultiyNo + "'";
                Query = Query + ",CashReceived='" + objsales.CashReceived + "'";
                //Query = Query + ",ProductReturn ='" + objsales.ProductReturn + "'";
                Query = Query + "  Where SaleID =" + objsales.SaleID;
                //Query = Query + " Select @@IDENTITY as SaleID";
                General.ExecuteNonQuery(Query);
                //objsales.SaleID = int.Parse(General.FetchData(Query).Rows[0]["SaleID"].ToString());
                Query = "";
                Query = "Delete from SaleDetail where SaleID = " + objsales.SaleID;
                
                foreach (SaleDetail ass in lstSaleDetail)
                {
                    Query = Query + " Insert into SaleDetail(SaleID,ProductID,Price,Quantity,Discount,Remarks) Values('" + objsales.SaleID + "','" + ass.ProductID + "','" + ass.Price + "','" + ass.Quantity + "','" + ass.Discount + "','" + ass.Remarks + "')";
                }
                if (Query != "")
                {
                    General.ExecuteNonQuery(Query);
                }
                return Json("true");
            }
            catch
            {
                return Json("False");
            }
        }
        // GET: SaleInfo/Delete/5
        public ActionResult Delete(int id)
        {
            string sql = @"Delete From SaleInfo where SaleID=" + id;
            sql = sql + "Delete From SaleDetail where SaleID=" + id;
            General.ExecuteNonQuery(sql);
            return Json("Success");
        }

        // POST: SaleInfo/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
