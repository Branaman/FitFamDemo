using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 
namespace ExcitedEmu.Models
{
    public class Activity : BaseEntity
    {
        public int idactivities {get;set;}
        [Required]
        public string title { get; set; }
        [Required]
        public string description {get;set;}
        [Required]
        public DateTime date {get;set;}
        [Required]
        public int progress {get;set;}
        [Required]
        public int users_idusers {get;set;}
        public int events_idevents {get;set;}
    }
}