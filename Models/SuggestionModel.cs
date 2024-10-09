using Microsoft.Build.Framework;

namespace SuggestionBox.Models
{
    public class SuggestionModel
    {
        [Required]
        public string Text { get; set; }

        public IFormFile? File { get; set; }
    }
}
