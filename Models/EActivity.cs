using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 
namespace ExcitedEmu.Models
{
    public class EActivity : BaseEntity
    {
        [Required]
        public string participant { get; set; }
        [Required]
        public int progress { get; set; }
        [Required]
        public string title { get; set; }
    }
}