using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    [Table(Name ="Table")]
    public class Abiturient
    {
        [Column(Name="FIO")]
        public string FIO { get; set; }
        [Column(Name = "Specialization")]
        public string Specialization { get; set; }
        [Column(Name = "Problems")]
        public string Problems { get; set; }
        [Column(Name = "Consent")]
        public bool Consent { get; set; }
    }
}
