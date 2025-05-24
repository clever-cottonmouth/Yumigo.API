using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yumigo.API.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }= string.Empty;
        public string Category { get; set; } = string.Empty;
        public string SpecialTag {  get; set; } = string.Empty;
        [Range(1,1000)]
        public double Price { get; set; }
        [Required]
        public string Image {  get; set; } = string.Empty;
    }
}
