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

        public ActionResult Index1()
        {

            return View();
        }


        public JsonResult GetOrdersWithDT(JQueryDataTableParamModel param)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            var totalrecored = db.Orders.Count();
            var data = new List<Order>();
            if (param.sSearch == "" && param.sSearch == null)
            {
                data = db.Orders.OrderBy(a => a.OrderId)
                    //.Skip(param.iDisplayStart)
                    //.Take(param.iDisplayLength)
                    .ToList();
            }
            else
            {
                data = db.Orders.OrderBy(a => a.OrderId)
                    .Where(r => r.OrderId.ToString() == param.sSearch || r.OrderNumber.ToString() == param.sSearch || (r.InternalNotes != null ? r.InternalNotes.Contains(param.sSearch) : r.InternalNotes == param.sSearch))
                    //.Skip(param.iDisplayStart)
                    //.Take(param.iDisplayLength)
                    .ToList();
            }
            var isOrderIdSortable = Convert.ToBoolean(Request["bSortable_0"]);
            var isOrderNumberSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isCustomerInstructionsSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isOrderTotalSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isOrderDateSortable = Convert.ToBoolean(Request["bSortable_4"]);


            Func<Order, string> orderingFunction = (c =>
                sortColumnIndex == 0 && isOrderIdSortable ? c.OrderId.ToString() :
                sortColumnIndex == 1 && isOrderNumberSortable ? c.OrderNumber.ToString() :
                sortColumnIndex == 2 && isCustomerInstructionsSortable ? c.CustomerInstructions.ToString() :
                sortColumnIndex == 3 && isOrderTotalSortable ? c.OrderTotal.ToString() :
                sortColumnIndex == 4 && isOrderDateSortable ? c.OrderDate.ToString() : "");

            var sortedResult = new List<Order>();
            if (sortDirection == "asc")
                sortedResult = data.OrderBy(orderingFunction).Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
            else
                sortedResult = data.OrderByDescending(orderingFunction).Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();


            var result = from c in sortedResult
                         select new[]
                         {
                          Convert.ToString( c.OrderId ),  // 0     
                          Convert.ToString( c.OrderNumber ),  // 1     
                          Convert.ToString( c.CustomerInstructions ),  // 2     
                          Convert.ToString( c.OrderTotal ),  // 3     
                          Convert.ToString( c.OrderDate ),  // 4    
                                                   };
            return Json(new
            {
                aaData = result,
                iTotalRecords = totalrecored,
                iTotalDisplayRecords = totalrecored
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrdersWithDT1()
        {
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                // Loading.   
                List<Order> data = db.Orders.ToList();
                // Total record count.   
                int totalRecords = data.Count;
                // Verification.   
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search   
                    data = data.Where(p => p.OrderId.ToString().ToLower().Contains(search.ToLower()) ||
                        p.OrderNumber.ToString().Contains(search.ToLower()) ||
                        p.OrderDate.ToString().Contains(search.ToLower()) ||
                        p.OrderTotal.ToString().Contains(search.ToLower())
                     ).ToList();
                }
                //Sorting.
                //if (!(string.IsNullOrEmpty(order) && string.IsNullOrEmpty(orderDir)))
                //{
                //    data = data.OrderBy(order + " " + orderDir).ToList();
                //}
                int recFilter = data.Count;
                data = data.Skip(startRec).Take(pageSize).ToList();
                var modifiedData = data.Select(d =>
                    new
                    {
                        d.OrderId,
                        d.OrderNumber,
                        d.CustomerInstructions,
                        d.OrderDate,
                        d.OrderTotal
                    }
                    );
                // Loading drop down lists.   
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = modifiedData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info   
                Console.Write(ex);
            }
            // Return info.   
            return result;
        }
    }
}
