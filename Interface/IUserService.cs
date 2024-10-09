using SuggestionBox.Models;

namespace SuggestionBox.Interface
{
    public interface IUserService
    {
        public User Login(LoginViewModel loginViewModel);
        public Task<bool> SaveSuggestion(SuggestionModel suggestionModel);
        public Suggestion GetSuggestionById(long id);
        public List<Suggestion> GetSuggestionList();
        public bool UpdateSuggestion(SaveCommentViewModel model);
    }
}
