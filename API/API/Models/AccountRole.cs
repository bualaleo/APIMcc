using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("Tb_Tr_AccRole")]
    public class AccountRole
    {
        
        public string NIK { get; set; }
        [JsonIgnore]
        public virtual Account Account { get; set; }
        
        public int IdRole { get; set; }
        [JsonIgnore]
        public virtual Role Role { get; set; }
    }
}
