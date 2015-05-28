using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace PaavoInsurances
{
    public class ClientTable
    { 
        [PrimaryKey]
        public string Id { get; set; }
        [MaxLength(50), Unique]
        public string securityId { get; set; }
        [MaxLength(50)]
        public string bonusCardNumber { get; set; }       
    }
}
