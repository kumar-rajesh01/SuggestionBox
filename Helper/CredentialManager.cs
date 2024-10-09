using CredentialManagement;
using log4net;

namespace SuggestionBox.Helper
{
    public class CredentialManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CredentialManager));

        public string UserName { get; set; }
        public string Password { get; set; }

        public static string ValidateCredential()
        {
            var key = string.Empty;
            try
            {
                var credential = new Credential { Target = "SuggestionBox_Key" };
                if (credential.Load())
                {
                    if (!string.IsNullOrEmpty(credential.Username) && credential.Username == "SuggestionBoxAdmin")
                    {
                        key = credential.Password;
                    }
                    else
                    {
                        log.Error("Credential didn't match");
                    }
                }
                else
                {
                    log.Error("Credential not found.");
                }
                return key;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while fetching credential from credential manager.", ex); _ = ex;
                return key;
            }
        }
    }
}
