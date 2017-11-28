using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 
namespace ExcitedEmu.Models
{
    public class Participants : BaseEntity
    {
        public int events_idevents { get; set; }
        public int users_idusers { get; set; }
        public int progress { get; set; }
        [Required]
        public int goal { get; set; }
        public Participants()
        {
            progress = 0;
        }
    }
}