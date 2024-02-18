using Helios.Web.Storage;
using Suggestor;

namespace Helios.Web.Util
{
    public class RegisterUtil
    {
        public static bool ValidateNameResponse(ref string errorType, ref string errorMessage, ref List<string> suggestions, string checkName, StorageContext _ctx)
        {
            if (string.IsNullOrEmpty(checkName))
            {
                errorType = "error";
                errorMessage = "Name cannot be blank";

                return false;
            }
            else
            {
                int nameCode = ValidateNameCode(checkName, _ctx);

                switch (nameCode)
                {
                    case 0:
                        {
                            errorType = "name_available";
                            break;
                        }
                    case 1:
                        {
                            errorType = "error";
                            errorMessage = "Name is longer than 16 characters";
                            break;
                        }
                    case 2:
                        {
                            errorType = "error";
                            errorMessage = "Name must not be shorter than 2 characters";
                            break;
                        }
                    case 4:
                        {
                            var suggestorSettings = SuggestorService.DefaultSettings;
                            suggestorSettings.MaximumWordLength = 16;

                            errorType = "already_exists";

                            suggestions = SuggestorService.GetSuggestions(checkName, suggestorSettings, existsCallback: (checkName) =>
                            {
                                return _ctx.AvatarData.Any(x => x.Name.ToLower() == checkName.ToLower());
                            });

                            break;
                        }
                    case 3:
                        {
                            errorType = "error";
                            errorMessage = "Name contains invalid characters";
                            break;
                        }
                }

                return nameCode == 0;
            }
        }

        public static int ValidateNameCode(string name, StorageContext _db)
        {
            int nameCheckCode = 0;

            if (_db.AvatarData.Any(x => x.Name == name))
            {
                nameCheckCode = 4;
            }
            else if (string.IsNullOrEmpty(name) || name.Length < 2)
            {
                nameCheckCode = 2;
            }
            else if (name.Length > 16)
            {
                nameCheckCode = 1;
            }
            else if (name.Contains(" ") || !ValidateNameCharacters(name.ToLower()) || name.ToUpper().Contains("MOD-"))
            {
                nameCheckCode = 3;
            }

            return nameCheckCode;
        }

        public static bool ValidateNameCharacters(string input)
        {
            string validCharacters = "1234567890qwertyuiopasdfghjklzxcvbnm-=?!@:.";

            foreach (char c in input)
            {
                if (!validCharacters.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
