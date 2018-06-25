using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerSidePaginationWithDataTableMVC.Models
{
    public class JQueryDataTableParamModel
    {
        public int iColumns { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }
        public int iSortingCols { get; set; }
        public string sColumns { get; set; }
        public string sEcho { get; set; }
        public string sSearch { get; set; }
    }

    public class MYJSONTbl
    {
        public string sEcho { get; set; }
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public object aaData { get; set; }
    }
}
