namespace SuggestionBox.Models
{
    public class SaveCommentViewModel
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public bool? IsResolved { get; set; }
    }
}
