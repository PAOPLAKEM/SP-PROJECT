using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class LOGCAM
    {
        [Key]
        public string CameraNo { get; set; } 
        [Required]
        public string Location { get; set; }
        
    }

}