using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO
{
    public class UserMood
    {
        [Required]
        public int UserId { get; set; }

        [StringLength(5000)]
        public string Comments { get; set; }

        [Required]
        public int MoodId { get; set; }
    }
}