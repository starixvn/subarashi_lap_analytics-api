using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Fanex.Athena.AnalyticsAPI.Common.Attributes;
using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Models.DataDomain;
using Fanex.Athena.AnalyticsAPI.Common.Resources.Lang;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly ILangResourceService _langResourceService;

        public TranslationService(ILangResourceService langResourceService)
        {
            _langResourceService = langResourceService;
        }

        public void Translate<T>(IEnumerable<T> originalItems, string translationType = null)
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
                    if (enumerable == null)
                    {
                        BindTranslateValue(originalItem, type, prop, translationType);
                    }
                }
            }
        }

        private void BindTranslateValue<T>(T originalItem, Type type, PropertyInfo prop, string translationType)
        {
            var attributeInfo = Attribute.GetCustomAttribute(prop, typeof(TranslationAttribute)) as TranslationAttribute;
            if (attributeInfo != null)
            {
                var id = GetTranslationId(originalItem, type, attributeInfo);
                var defaultValue = prop.GetValue(originalItem)?.ToString() ?? string.Empty;
                translationType ??= attributeInfo.TranslationType;

                var tranlatedValue = _langResourceService.GetName(translationType, id, defaultValue);

                if (translationType == TranslationConst.BettypeType)
                {
                    const string includePattern = @"\s+\(\w+\)$";
                    tranlatedValue = GetTranslateValueWithIncludePattern(defaultValue, tranlatedValue, includePattern);
                }

                if (translationType == TranslationConst.SportType
                    && originalItem is ISabaAble sabaModel
                    && sabaModel.IsSaba)
                {
                    tranlatedValue = $"{ViewResource.Saba}{tranlatedValue}";
                }

                prop.SetValue(originalItem, tranlatedValue);
            }
        }

        private static string GetTranslationId<T>(T originalItem, Type type, TranslationAttribute attributeInfo)
        {
            var id = type.GetProperty(attributeInfo.PropertyId)?
                      .GetValue(originalItem)?
                      .ToString();
            id = id?.Split('@')?.LastOrDefault();
            return id;
        }

        private static string GetTranslateValueWithIncludePattern(string defaultValue, string tranlatedValue, string includePattern)
        {
            var replacements = Regex.Split(defaultValue, includePattern);
            foreach (var replacement in replacements)
            {
                defaultValue = string.IsNullOrEmpty(replacement) ? defaultValue : defaultValue.Replace(replacement, tranlatedValue);
            }
            return defaultValue;
        }
    }
}