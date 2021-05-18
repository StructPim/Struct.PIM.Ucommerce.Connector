using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Struct.PIM.Api.Models.Attribute;
using Struct.PIM.Api.Models.Shared;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim.Helpers
{
    public static class AttributeRenderHelper
    {
        public static List<RenderedValue> RenderAttributeValue(Struct.PIM.Api.Models.Attribute.Attribute attribute, object value, string cultureCode, bool unfold)
        {
            if (value == null)
            {
                return new List<RenderedValue>();
            }

            if (attribute is TextAttribute)
            {
                var v = RenderTextAttribute(attribute as TextAttribute, value, cultureCode);
                return v != null ? v.AsList() : new List<RenderedValue>();
            }
            if (attribute is NumberAttribute)
            {
                var v = RenderNumberAttribute(attribute as NumberAttribute, value, cultureCode);
                return v != null ? v.AsList() : new List<RenderedValue>();
            }
            if (attribute is BooleanAttribute)
            {
                var v = RenderBooleanAttribute(attribute as BooleanAttribute, value, cultureCode);
                return v != null ? v.AsList() : new List<RenderedValue>();
            }
            if (attribute is FixedListAttribute)
            {
                return RenderFixedlistAttribute(attribute as FixedListAttribute, value, cultureCode, unfold);
            }
            if (attribute is MediaAttribute)
            {
                var v = RenderMediaAttribute(attribute as MediaAttribute, value, cultureCode);
                return v != null ? v.AsList() : new List<RenderedValue>();
            }
            if (attribute is ListAttribute)
            {
                var v = RenderListAttribute(attribute as ListAttribute, value, cultureCode);
                return v != null ? v.AsList() : new List<RenderedValue>();
            }
            if (attribute is ProductReferenceAttribute)
            {
                var attr = attribute as ProductReferenceAttribute;
                var v = RenderReferenceAttribute<int>(attribute, attr.AllowMultiple, value, cultureCode);
            }
            if (attribute is VariantReferenceAttribute)
            {
                var attr = attribute as VariantReferenceAttribute;
                var v = RenderReferenceAttribute<int>(attribute, attr.AllowMultiple, value, cultureCode);
            }
            if (attribute is CategoryReferenceAttribute)
            {
                var attr = attribute as CategoryReferenceAttribute;
                var v = RenderReferenceAttribute<int>(attribute, attr.AllowMultiple, value, cultureCode);
            }
            if (attribute is CollectionReferenceAttribute)
            {
                var attr = attribute as CollectionReferenceAttribute;
                var v = RenderReferenceAttribute<Guid>(attribute, attr.AllowMultiple, value, cultureCode);
            }
            if (attribute is AttributeReferenceAttribute)
            {
                var attr = attribute as AttributeReferenceAttribute;
                var v = RenderReferenceAttribute<List<Guid>>(attribute, attr.AllowMultipleValues, value, cultureCode);
            }
            if (attribute is DateTimeAttribute)
            {
                var v = RenderDateTimeAttribute(attribute as DateTimeAttribute, value, cultureCode);
                return v != null ? v.AsList() : new List<RenderedValue>();
            }

            if (attribute is ComplexAttribute)
            {
                return RenderComplexAttribute(attribute as ComplexAttribute, value, cultureCode, unfold);
            }

            throw new InvalidOperationException("Cannot render attribute value. Unknown attribute type");
        }

        private static RenderedValue RenderTextAttribute(TextAttribute attribute, object value, string cultureCode)
        {
            if (value == null)
            {
                return null;
            }

            var renderedValue = string.Empty;
            if (attribute.Localized)
            {
                var localizedValue = value.As<List<LocalizedData<string>>>();

                if (localizedValue != null)
                {
                    renderedValue = localizedValue.Get(cultureCode);
                }
                else
                {
                    renderedValue = string.Empty;
                }
            }
            else
            {
                renderedValue = value as string;
            }

            return new RenderedValue(attribute, cultureCode)
            {
                Value = new List<string> { renderedValue },
                Unit = attribute.Unit
            };
        }

        private static RenderedValue RenderNumberAttribute(NumberAttribute attribute, object value, string cultureCode)
        {
            if (value == null)
            {
                return null;
            }
            var renderedValue = string.Empty;
            if (attribute.Localized)
            {
                if (attribute.NumberOfDecimals == 0)
                {
                    var localizedValue = value.As<List<LocalizedData<int?>>>();
                    renderedValue = localizedValue.Get(cultureCode)?.ToString(CultureInfo.GetCultureInfo(cultureCode));
                }
                else
                {
                    var localizedValue = value.As<List<LocalizedData<decimal?>>>();
                    renderedValue = localizedValue.Get(cultureCode)?.ToString(CultureInfo.GetCultureInfo(cultureCode));
                }
            }
            else
            {
                if (attribute.NumberOfDecimals == 0)
                {
                    renderedValue = ((long)value).ToString(CultureInfo.GetCultureInfo(cultureCode));
                }
                else
                {
                    renderedValue = ((double)value).ToString(CultureInfo.GetCultureInfo(cultureCode));
                }
            }

            return new RenderedValue(attribute, cultureCode)
            {
                Value = new List<string> { renderedValue },
                Unit = attribute.Unit
            };
        }

        private static RenderedValue RenderBooleanAttribute(BooleanAttribute attribute, object value, string cultureCode)
        {
            if (value == null)
            {
                return null;
            }
            var renderedValue = string.Empty;
            if (attribute.Localized)
            {
                var localizedValue = value.As<List<LocalizedData<bool?>>>();
                var v = localizedValue.Get(cultureCode);
                renderedValue = v == true ? "Yes" : "No";
            }
            else
            {
                renderedValue = (value as bool?) == true ? "Yes" : "No";
            }

            return new RenderedValue(attribute, cultureCode)
            {
                Value = new List<string> { renderedValue }
            };
        }

        private static List<RenderedValue> RenderFixedlistAttribute(FixedListAttribute attribute, object value, string cultureCode, bool unfold)
        {
            if (value == null)
            {
                return new List<RenderedValue>();
            }

            if (attribute.AllowMultipleValues)
            {
                var result = new Dictionary<string, RenderedValue>();
                foreach (var selectedValue in JArray.FromObject(value))
                {
                    var renderedValues = RenderAttributeValue(attribute.ReferencedAttribute, selectedValue, cultureCode, unfold);
                    foreach (var renderedValue in renderedValues)
                    {
                        renderedValue.AttributeAliases[0] = attribute.Alias;
                        renderedValue.AttributeUids[0] = attribute.Uid;
                        if (!result.ContainsKey(renderedValue.AttributeAliasPath))
                        {
                            result.Add(renderedValue.AttributeAliasPath, renderedValue);
                        }
                        else
                        {
                            result[renderedValue.AttributeAliasPath].Value.AddRange(renderedValue.Value);
                        }
                    }
                }
                return result.Values.ToList();
            }
            else
            {
                var renderedValues = RenderAttributeValue(attribute.ReferencedAttribute, value, cultureCode, unfold);
                foreach (var renderedValue in renderedValues)
                {
                    renderedValue.AttributeAliases[0] = attribute.Alias;
                    renderedValue.AttributeUids[0] = attribute.Uid;
                }
                return renderedValues;
            }
        }

        private static RenderedValue RenderMediaAttribute(MediaAttribute attribute, object value, string cultureCode)
        {
            if (value == null)
            {
                return null;
            }

            if (attribute.AllowMultiselect)
            {
                if (attribute.Localized)
                {
                    var localizedData = value.As<List<LocalizedData<List<string>>>>().Get(cultureCode);
                    return new RenderedValue(attribute, cultureCode) { Value = localizedData ?? new List<string>() };
                }
                else
                {
                    return new RenderedValue(attribute, cultureCode) { Value = value.As<List<string>>() };
                }
            }
            else
            {
                if (attribute.Localized)
                {
                    var localizedData = value.As<List<LocalizedData<string>>>().Get(cultureCode);
                    return new RenderedValue(attribute, cultureCode) { Value = localizedData.AsList() };
                }
                else
                {
                    return new RenderedValue(attribute, cultureCode) { Value = new List<string> { value as string } };
                }
            }
        }

        private static RenderedValue RenderDateTimeAttribute(DateTimeAttribute attribute, object value, string cultureCode)
        {
            if (value == null)
            {
                return null;
            }

            var formatter = attribute.ShowTime ? "g" : "d";
            if (attribute.Localized)
            {
                var localizedData = value.As<List<LocalizedData<DateTimeOffset?>>>().Get(cultureCode);
                return new RenderedValue(attribute, cultureCode) { Value = localizedData?.ToString(formatter, CultureInfo.GetCultureInfo(cultureCode)).AsList() ?? new List<string>() };
            }
            else
            {
                return new RenderedValue(attribute, cultureCode) { Value = value.As<DateTimeOffset>().ToString(formatter, CultureInfo.GetCultureInfo(cultureCode)).AsList() };
            }
        }

        private static RenderedValue RenderListAttribute(ListAttribute attribute, object value, string cultureCode)
        {
            if (value == null)
            {
                return null;
            }

            var result = new List<string>();
            foreach (var item in JArray.FromObject(value))
            {
                var renderedValue = RenderAttributeValue(attribute.Template, item, cultureCode, false).First();
                if (renderedValue.Value != null && renderedValue.Value.Any() && !string.IsNullOrEmpty(renderedValue.Value.First()))
                {
                    result.Add(renderedValue.Value.First());
                }
            }
            return new RenderedValue(attribute, cultureCode) { Value = result };
        }

        private static RenderedValue RenderReferenceAttribute<T>(Struct.PIM.Api.Models.Attribute.Attribute attribute, bool allowMultiple, object value, string cultureCode)
        {
            if (value == null)
            {
                return null;
            }
            
            if (allowMultiple)
            {
                var v = value.As<List<T>>().Select(x => x.ToString()).ToList();
                return new RenderedValue(attribute, cultureCode) { Value = v };
            }
            else
            {
                return new RenderedValue(attribute, cultureCode) { Value = value.As<T>().AsList().Select(x => x.ToString()).ToList() };
            }
        }

        private static List<RenderedValue> RenderComplexAttribute(ComplexAttribute attribute, object value, string cultureCode, bool unfold)
        {
            if (value == null)
            {
                return new List<RenderedValue>();
            }

            var result = new List<RenderedValue>();

            var obj = JObject.FromObject(value).ToObject<Dictionary<string, dynamic>>();
            var subAttributesToRender = attribute.RenderValuesForAttributeFieldUids?.Any() ?? false ? attribute.RenderValuesForAttributeFieldUids : attribute.SubAttributes.Select(x => x.Uid).ToList();

            var subValuesRendered = new List<string>();
            foreach (var subAttributeUid in subAttributesToRender)
            {
                var subAttribute = attribute.SubAttributes.FirstOrDefault(x => x.Uid == subAttributeUid);
                var subValueRendered = (List<RenderedValue>)RenderAttributeValue(subAttribute, obj[subAttribute.Alias], cultureCode, false);
                subValuesRendered.AddRange(subValueRendered.FirstOrDefault()?.Value ?? new List<string>());
            }
            result.Add(new RenderedValue(attribute, cultureCode) { Value = string.Join(attribute.RenderedValueSeparator ?? "", subValuesRendered).AsList() });

            if (unfold)
            {
                foreach (var subAttribute in attribute.SubAttributes)
                {
                    var renderedValues = (List<RenderedValue>)RenderAttributeValue(subAttribute, obj[subAttribute.Alias], cultureCode, true);
                    foreach (var renderedValue in renderedValues)
                    {
                        renderedValue.AttributeUids = renderedValue.AttributeUids.Prepend(attribute.Uid).ToList();
                        renderedValue.AttributeAliases = renderedValue.AttributeAliases.Prepend(attribute.Alias).ToList();
                    }
                    result.AddRange(renderedValues);
                }
            }

            return result;
        }
    }

    public class RenderedValue
    {
        public RenderedValue(Struct.PIM.Api.Models.Attribute.Attribute attribute, string cultureCode)
        {
            AttributeGroupId = attribute.AttributeGroupId;
            AttributeScopeUid = attribute.AttributeScope;
            AttributeName = attribute.Name.Get(cultureCode);
            AttributeAliases = new List<string> { attribute.Alias };
            AttributeUids = new List<Guid> { attribute.Uid };
        }
        public List<Guid> AttributeUids { get; set; }
        public List<string> AttributeAliases { get; set; }
        public int? AttributeGroupId { get; set; }
        public Guid AttributeScopeUid { get; set; }
        public string AttributeName { get; set; }
        public string Unit { get; set; }
        public List<string> Value { get; set; }
        public string AttributeUidPath { get { return string.Join(".", AttributeUids); } }
        public string AttributeAliasPath { get { return string.Join(".", AttributeAliases); } }
    }
}
