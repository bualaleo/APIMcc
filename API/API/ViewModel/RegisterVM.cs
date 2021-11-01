using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModel
{
    public class RegisterVM
    {
        

        [Required]
        public string NIK { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FullName { get; set; }
        
        public string Phone { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public int Salary { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]

        public Gender Gender { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Degree { get; set; }
        [Required]
        public string GPA { get; set; }
        [Required]
        public int IdUniversity { get; set; }
        public string NamaUniversity { get; set; }
        [Required]
        public int IdRole { get; set; }
        
    }
    public enum Gender
    {
        Male, Female
    }
}
