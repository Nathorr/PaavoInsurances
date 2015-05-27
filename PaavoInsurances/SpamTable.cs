using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace PaavoInsurances
{
    public class SpamTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(30)]
        public string name { get; set; }
        [MaxLength(30)]
        public string email { get; set; }
        [MaxLength(30)]
        public string phone { get; set; }
        [MaxLength(30)]
        public bool home { get; set; }
        [MaxLength(30)]
        public bool health { get; set; }
        [MaxLength(30)]
        public bool vehicle { get; set; }
        [MaxLength(30)]
        public bool life { get; set; }
        [MaxLength(30)]
        public bool infant { get; set; }
        [MaxLength(30)]
        public bool pets { get; set; }


    }
}
