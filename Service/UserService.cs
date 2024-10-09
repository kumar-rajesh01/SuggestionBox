using GCrypt;
using log4net;
using Microsoft.EntityFrameworkCore;
using SuggestionBox.Data;
using SuggestionBox.Helper;
using SuggestionBox.Interface;
using SuggestionBox.Models;

namespace SuggestionBox.Service
{
    public class UserService : IUserService
    {
        private readonly ILog log = LogManager.GetLogger(typeof(UserService));
        private AppDbContext _dbContext;
        private IHttpContextAccessor _httpContextAccessor;

        public UserService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public User Login(LoginViewModel loginViewModel)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.Email == loginViewModel.Email);
                if (user == null)
                    throw new Exception("User doesnot exists.");
                var userData = _dbContext.Users.FirstOrDefault(a => a.Email.Equals(loginViewModel.Email) && a.Password == GCrypter.Encrypt(loginViewModel.Password) && a.IsActive == true);

				return userData;
            }
            catch (Exception ex)
            {
                _ = ex;
                throw;
            }
        }

        public async Task<bool> SaveSuggestion(SuggestionModel model)
        {
            try
            {
                var suggestion = new Suggestion
                {
                    Ip = GCrypter.Encrypt(CommonHelper.GetRequestIP(_httpContextAccessor)),
                    Text = GCrypter.Encrypt(model.Text),
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                };
                if(model.File != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.File.CopyToAsync(memoryStream);
                        var fileBytes = memoryStream.ToArray();
                        var base64String = Convert.ToBase64String(fileBytes);
                        suggestion.File = GCrypter.Encrypt(base64String);
                        suggestion.FileName = GCrypter.Encrypt(model.File.FileName);
                    }
                }
                var suggest = _dbContext.Suggestions.Add(suggestion);
                _dbContext.SaveChanges();
                return suggestion.SuggestionId > 0;
            }
            catch (Exception ex)
            {
                _ = ex;
                throw;
            }
        }

        public Suggestion GetSuggestionById(long id)
        {
            try
            {
                return new Suggestion(_dbContext.Suggestions.FirstOrDefault(x => x.SuggestionId == id));
            }
            catch (Exception ex)
            {
                log.Error("Error occured while getting suggestion detail"); _ = ex;
                throw;
            }
        }

        public List<Suggestion> GetSuggestionList()
        {
            try
            {
                return _dbContext.Suggestions.Select(x => new Suggestion(x)).ToList().OrderBy(x => x.SuggestionId).ToList();
            }
            catch (Exception ex)
            {
                log.Error("Exception occured while getting suggestion list.", ex);
                _ = ex; throw;
            }
        }

        public bool UpdateSuggestion(SaveCommentViewModel model)
        {
            try
            {
                var suggestion = _dbContext.Suggestions.FirstOrDefault(x => x.SuggestionId == model.Id);
                if(suggestion != null)
                {
                    suggestion.IsReviewed = model.IsResolved.HasValue && !suggestion.IsReviewed ? model.IsResolved.Value : suggestion.IsReviewed;
                    suggestion.Comment = !string.IsNullOrEmpty(model.Comment) && string.IsNullOrEmpty(suggestion.Comment) ? GCrypter.Encrypt(model.Comment) : suggestion.Comment;
                    suggestion.DateUpdated = DateTime.Now;
                }
                _dbContext.SaveChanges();
                return suggestion?.SuggestionId > 0;
            }
            catch (Exception ex)
            {
                log.Error("Exception occured while updating suggestion.", ex);
                _ = ex; throw;
            }
        }
    }
}
