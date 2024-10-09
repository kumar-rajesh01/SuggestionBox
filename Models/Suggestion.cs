using GCrypt;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuggestionBox.Models
{
    [Table("Suggestion")]
    public class Suggestion
    {
        public Suggestion() { }
        public Suggestion(Suggestion model) 
        {
            if (model != null)
            {
                this.SuggestionId = model.SuggestionId;
                this.Text = !string.IsNullOrEmpty(model.Text) ? GCrypter.Decrypt(model.Text) : model.Text;
                this.Ip = !string.IsNullOrEmpty(model.Ip) ? GCrypter.Decrypt(model.Ip) : model.Ip;
                this.File = !string.IsNullOrEmpty(model.File) ? GCrypter.Decrypt(model.File) : model.File;
                this.FileName = !string.IsNullOrEmpty(model.FileName) ? GCrypter.Decrypt(model.FileName) : model.FileName;
                this.Comment = !string.IsNullOrEmpty(model.Comment) ? GCrypter.Decrypt(model.Comment) : model.Comment;
                this.IsReviewed = model.IsReviewed;
                this.DateCreated = model.DateCreated;
                this.DateUpdated = model.DateUpdated;
            };
        }

        [Key]
        public long SuggestionId { get; set; }
        public string Text { get; set; }
        public string Ip { get; set; }
        public string? File { get; set; }
        public string? FileName { get; set; }
        public string? Comment { get; set; }
        public bool IsReviewed { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
