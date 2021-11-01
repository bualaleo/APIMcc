using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("Tb_M_Education")]
    public class Education
    {
        [Key]
        public int IdEducation { get; set; }
        public string Degree { get; set; }
        public string GPA { get; set; }
        public int IdUniversity { get; set; }

        [JsonIgnore]
        public virtual ICollection<Profiling> Profilings { get; set; }

        [JsonIgnore]
        public virtual University University { get; set; }
    }
}
