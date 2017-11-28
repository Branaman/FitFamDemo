using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 
namespace ExcitedEmu.Models
{
    public class Event : BaseEntity
    {
        public int idevents {get;set;}
        [Required]
        public string title { get; set; }
        [Required]
        public string description {get;set;}
        [Required]
        public DateTime date {get;set;}
        [Required]
        public DateTime time {get;set;}
        [Required]
        public string duration {get;set;}
        [Required]
        public string timeMod {get;set;}
        [Required]
        public int goal {get;set;}
        public int users_idusers {get;set;}
        public int participants {get;set;}
        public Event()
        {
            participants = 0;
        }
    }
}