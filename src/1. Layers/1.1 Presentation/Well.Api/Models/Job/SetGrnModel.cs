using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PH.Well.Api.Models.Job
{
    public class SetGrnModel
    {
        /// <summary>
        /// Job Id
        /// </summary>
        public int Id { get; set; }

        public string Grn { get; set; }
    }
}