using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronicle.Security.Classes.Objects
{
    public class Class
    {

        [ReadOnly(true)]
        [Category("Audit")]
        [Description("Unique Internal Object Identifier")]
        [DisplayName("Object ID")]
        public int classID { get; set; }

        

        [Category("Audit")]
        [DisplayName("Created By")]
        [ReadOnly(true)]
        public string CreatedBy { get; set; }

        [Category("Audit")]
        [DisplayName("Created Date")]
        [ReadOnly(true)]
        public DateTime CreatedDate { get; set; }

        [Category("Audit")]
        [DisplayName("Updated By")]
        [ReadOnly(true)]
        public string UpdatedBy { get; set; }

        [Category("Audit")]
        [DisplayName("Updated Date")]
        [ReadOnly(true)]
        public DateTime UpdatedDate { get; set; }

    }
}
