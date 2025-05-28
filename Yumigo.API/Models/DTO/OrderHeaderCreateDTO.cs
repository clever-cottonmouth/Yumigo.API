using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yumigo.API.Models.DTO
{
    public class OrderHeaderCreateDTO
    {
        public int OrderHeaderId { get; set; }
        [Required]
        public string PickUpName { get; set; } = string.Empty;
        [Required]
        public string PickUpPhoneNumber { get; set; } = string.Empty;
        [Required]
        public string PickUpEmail { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; } = string.Empty;
        public double OrderTotal { get; set; }
        public int TotalItem { get; set; }
        public List<OrderDetailCreateDTO> OrderDetailsDTO { get; set; } = new();
    }
}
