using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace MvcAngular.Web.Repository
{
    public class Row
    {
        public DateTime Timestamp { get; set; }
        public string CallerId { get; set; }
        public string ConsoleName { get; set; }
        public string EmployeeName { get; set; }
    }
}