using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace MvcAngular.Web.Repository
{
    public class Row
    {
        public DateTime Fld_Timestamp { get; set; }
        public string Fld_CallerId { get; set; }
        public string AcdExt { get; set; }
        public string Username { get; set; }
    }
}