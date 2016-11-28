using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.Models
{
    public class WSystemViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Code { get; set; } //system code
        [Required]
        public string Name { get; set; } //system name

        //public ICollection<ProductionViewModel> Productions { get; set; }
    }
}