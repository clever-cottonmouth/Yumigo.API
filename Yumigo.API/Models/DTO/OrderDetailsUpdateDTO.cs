using System.ComponentModel.DataAnnotations;

namespace Yumigo.API.Models.DTO
{
    public class OrderDetailsUpdateDTO
    {
        [Required]
        public int OrderDetailId { get; set; }
        [Required]
        [Range(1,5)]
        public int Rating { get; set; }
    }
}
