using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("Tb_M_Universitas")]
    public class University
    {
        [Key]
        public int IdUniversity { get; set; }
        public string NamaUniversity { get; set; }

        [JsonIgnore]
        public virtual ICollection<Education> Educations { get; set; }
    }
}
