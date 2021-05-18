using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Struct.PIM.Api.Models.Attribute;
using Struct.PIM.Api.Models.Product;
using Struct.PIM.Api.Models.Shared;
using Struct.PIM.Api.Models.Variant;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Helpers;

namespace Struct.PIM.Ucommerce.Connector.Integration
{
    public static class Extensions
    {
        public static T As<T>(this object obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        public static T Get<T>(this List<LocalizedData<T>> localizedData, string cultureCode)
        {
            var v = localizedData?.FirstOrDefault(x => x.CultureCode == cultureCode);
            return v != null ? v.Data : default(T);
        }

        public static string Get(this Dictionary<string, string> localizedData, string cultureCode)
        {
            string value = null;
            localizedData?.TryGetValue(cultureCode, out value);
            return value;
        }

        public static T Get<T>(this List<SegmentedLocalizedData<T>> segmentedLocalizedData, string cultureCode, string segment)
        {
            var v = segmentedLocalizedData?.FirstOrDefault(x => x.CultureCode == cultureCode && x.Segment == segment);
            return v != null ? v.Data : default(T);
        }

        public static IEnumerable<List<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int batchSize)
        {
            var batch = new List<TSource>(batchSize);
            foreach (var item in source)
            {
                batch.Add(item);
                if (batch.Count == batchSize)
                {
                    yield return batch;
                    batch = new List<TSource>();
                }
            }

            if (batch.Any()) yield return batch;
        }

        public static TValue GetValueOrFallback<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue fallback)
        {
            return dictionary.TryGetValue(key, out var value) ? value : fallback;
        }

        public static List<T> AsList<T>(this T obj)
        {
            return new List<T> { obj };
        }

        public static List<RenderedValue> Render(this Struct.PIM.Api.Models.Attribute.Attribute attribute, object value, string cultureCode, bool unFold = true)
        {
            return AttributeRenderHelper.RenderAttributeValue(attribute, value, cultureCode, unFold);
        }

        public static string RenderFirstValue(this Struct.PIM.Api.Models.Attribute.Attribute attribute, ProductAttributeValuesModel product, string cultureCode)
        {
            if (!product.Values.TryGetValue(attribute.Alias, out var productValue))
            {
                return null;
            }

            var renderedValues = attribute.Render((object)productValue, cultureCode, false);

            var firstValue = renderedValues.FirstOrDefault();
            if (firstValue == null)
            {
                return null;
            }

            return string.Join(", ", firstValue.Value.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        public static string RenderFirstValue(this Struct.PIM.Api.Models.Attribute.Attribute attribute, VariantAttributeValuesModel variant, string cultureCode)
        {
            if (!variant.Values.TryGetValue(attribute.Alias, out var productValue))
            {
                return null;
            }

            var renderedValues = attribute.Render((object)productValue, cultureCode, false);

            var firstValue = renderedValues.FirstOrDefault();
            if (firstValue == null)
            {
                return null;
            }

            return string.Join(", ", firstValue.Value.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        public static bool IsAnyLocalized(this Struct.PIM.Api.Models.Attribute.Attribute attribute)
        {
            if (attribute is ComplexAttribute)
            {
                return attribute.Localized || ((ComplexAttribute)attribute).SubAttributes.Any(IsAnyLocalized);
            }

            if (attribute is FixedListAttribute)
            {
                return attribute.Localized || IsAnyLocalized(((FixedListAttribute)attribute).ReferencedAttribute);
            }

            if (attribute is ListAttribute)
            {
                return attribute.Localized || IsAnyLocalized(((ListAttribute)attribute).Template);
            }

            return attribute.Localized;
        }
    }
}
