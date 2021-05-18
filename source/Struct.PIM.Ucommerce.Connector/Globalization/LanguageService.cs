using System.Collections.Generic;
using Ucommerce.Infrastructure.Globalization;

namespace Struct.PIM.Ucommerce.Connector.Globalization
{
    public class LanguageService : ILanguageService
    {
        public IList<Language> GetAllLanguages()
        {
            return new List<Language>()
            {
                new Language("en-GB", "en-GB"),
                new Language("da-DK", "da-DK")
            };
        }
    }
}
