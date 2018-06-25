using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ServerSidePaginationWithDataTableMVC;
using ServerSidePaginationWithDataTableMVC.Models;

namespace ServerSidePaginationWithDataTableMVC.Controllers
{
    public class OrdersController : Controller
    {
        private CommerceBitCustomerEntities db = new CommerceBitCustomerEntities();

        // GET: Orders
        public ActionResult Index()
        {
            return View();
        }

        public string GetOrderListUsingSP(JQueryDataTableParamModel param)
        {

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var SortColumn = "OrderId";
            var SortOrder = "desc";

            switch (sortColumnIndex)
            {
                case 0:
                    SortColumn = "OrderId";
                    break;

                case 1:
                    SortColumn = "OrderNumber";
                    break;


                case 2:
                    SortColumn = "CustomerId";
                    break;

                case 3:
                    SortColumn = "CustomerInstructions";
                    break;


                default:
                    SortColumn = "InternalNotes";
                    break;

            }


            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                SortOrder = "asc";
            else
                SortOrder = "desc";
            if (param.sSearch == null)
            {
                param.sSearch = "";
            }
            List<Order> Clientdata = new List<Order>();
            try
            {
                var Orders = db.GetOrderDetails(param.iDisplayStart, param.sSearch, param.iDisplayLength, SortColumn, SortOrder).ToList();
                if (Orders.Count > 0)
                {
                    Clientdata = Orders.Select(i => new Order
                    {
                        OrderId = i.OrderId,
                        OrderNumber = i.OrderNumber,
                        CustomerId = i.CustomerId,
                        CustomerInstructions = i.CustomerInstructions,
                        OrderDate = i.OrderDate,
                    }).AsQueryable().ToList();

                    MYJSONTbl _MYJSONTbl = new MYJSONTbl();
                    _MYJSONTbl.sEcho = param.sEcho;
                    _MYJSONTbl.iTotalRecords = Orders.Count() > 0 ? Orders[0].TotalPages : 0;
                    _MYJSONTbl.iTotalDisplayRecords = Orders.Count() > 0 ? Orders[0].TotalPages : 0;
                    _MYJSONTbl.aaData = Clientdata;

                    return Serialize(_MYJSONTbl);
                    //return Clientdata;
                }
                else
                {
                    MYJSONTbl _MYJSONTbl = new MYJSONTbl();
                    _MYJSONTbl.sEcho = param.sEcho;
                    _MYJSONTbl.iTotalRecords = Orders.Count() > 0 ? Orders[0].TotalPages : 0;
                    _MYJSONTbl.iTotalDisplayRecords = Orders.Count() > 0 ? Orders[0].TotalPages : 0;
                    _MYJSONTbl.aaData = Clientdata;

                    return Serialize(_MYJSONTbl);
                }
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                var error = ex.InnerException != null ? ex.InnerException.ToString() : "";
                error += ex.StackTrace != null ? ex.StackTrace.ToString() : "";
                //Email.ErrorManagement("ClientController", "GetOrderList", ex.Message != null ? ex.Message.ToString() : "", error, false, true);
                return null;
            }

        }

        public static string Serialize(object obj)
        {
            string result = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.None });
            return result;
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.ParentOrderId = new SelectList(db.Orders, "OrderId", "CustomerInstructions");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,OrderNumber,CustomerId,CustomerInstructions,InternalNotes,OrderDate,IsQuote,RowVersion,DateCreated,CreatedByUserId,DateUpdated,UpdatedByUserId,OrderTotal,ShippingTotal,DiscountTotal,TAX,GrandTotal,TaxRateId,PaymentStatus,ShipStatus,OrderStatus,OrderType,ParentOrderId,Channel,ChannelOrderid,InsuranceTotal,QBExported,IsTaxCollected,TaxCounty,MarketPlaceProfileID,ApplyPayments_ApplyPaymentId,MarketPlace_ItemID,CarrierXMLRequestInput,CarrierXMLResponseOutput,IsDeleted,PackageStatus,LablePrintingDate,LastChecked")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParentOrderId = new SelectList(db.Orders, "OrderId", "CustomerInstructions", order.ParentOrderId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentOrderId = new SelectList(db.Orders, "OrderId", "CustomerInstructions", order.ParentOrderId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,OrderNumber,CustomerId,CustomerInstructions,InternalNotes,OrderDate,IsQuote,RowVersion,DateCreated,CreatedByUserId,DateUpdated,UpdatedByUserId,OrderTotal,ShippingTotal,DiscountTotal,TAX,GrandTotal,TaxRateId,PaymentStatus,ShipStatus,OrderStatus,OrderType,ParentOrderId,Channel,ChannelOrderid,InsuranceTotal,QBExported,IsTaxCollected,TaxCounty,MarketPlaceProfileID,ApplyPayments_ApplyPaymentId,MarketPlace_ItemID,CarrierXMLRequestInput,CarrierXMLResponseOutput,IsDeleted,PackageStatus,LablePrintingDate,LastChecked")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentOrderId = new SelectList(db.Orders, "OrderId", "CustomerInstructions", order.ParentOrderId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
