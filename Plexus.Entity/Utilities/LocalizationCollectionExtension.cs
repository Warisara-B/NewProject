using System;
using Plexus.Entity.DTO;

namespace Plexus.Entity.Utilities
{
    public static class LocalizationCollectionExtension
    {
        public static T GetDefault<T>(this IEnumerable<T> localizations) where T : class
        {
            var localizedCollections = localizations as IEnumerable<LocalizationDTO>;

            var defaultLocalizedProperty = localizedCollections?.SingleOrDefault(x => x.Language == Database.Enum.LanguageCode.EN);
            if (defaultLocalizedProperty == null)
            {
                return localizations.First();
            }

            return defaultLocalizedProperty as T;
        }
    }
}