using Fanex.Athena.AnalyticsAPI.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly ILangResourceService _langResourceService;

        public TranslationService(ILangResourceService langResourceService)
        {
            _langResourceService = langResourceService;
        }

        public void Translate<T>(IEnumerable<T> originalItems, string langCode, string translationType = null)
            where T : class
        {
            if (originalItems == null || !originalItems.Any())
            {
                return;
            }

            var type = originalItems.ElementAt(0).GetType();
            var props = type.GetProperties();

            foreach (var originalItem in originalItems)
            {
                foreach (var prop in props)
                {
                    var enumerable = prop.GetValue(originalItem) as IEnumerable<object>;
                    if (enumerable != null)
                    {
                        Translate(enumerable, langCode, translationType);
                    }
                    else
                    {
                        BindTranslateValue(originalItem, type, prop, langCode, translationType);
                    }
                }
            }
        }

        private void BindTranslateValue<T>(T originalItem, Type type, PropertyInfo prop, string langCode, string translationType)
        {
            var attributeInfo = Attribute.GetCustomAttribute(prop, typeof(TranslationAttribute)) as TranslationAttribute;
            if (attributeInfo != null)
            {
                var id = type.GetProperty(attributeInfo.PropertyId)?
                         .GetValue(originalItem)?
                         .ToString();

                id = id?.Split('@')?.LastOrDefault();
                var defaultValue = prop.GetValue(originalItem)?.ToString();
                translationType = translationType ?? attributeInfo.TranslationType;

                var tranlatedValue = _langResourceService.GetName(translationType, id, langCode, defaultValue);
                //todo - bettype
                prop.SetValue(originalItem, tranlatedValue);
            }
        }
    }
}