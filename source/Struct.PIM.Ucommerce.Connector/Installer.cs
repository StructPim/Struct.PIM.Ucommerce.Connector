using System;
using System.Linq;
using Ucommerce.EntitiesV2;
using Ucommerce.EntitiesV2.Factories;

namespace Struct.PIM.Ucommerce.Connector
{
    public class Installer
    {
        public void CreateProductDefinitions()
        {
            CreatePimCategoryDefinition();
            var parentDefinition = CreatePimProductDefinition();
            CreateEventProductDefinition(parentDefinition);
            CreateFoodProductDefinition(parentDefinition);
            CreateWineProductDefinition(parentDefinition);
            CreateNonFoodProductDefinition(parentDefinition);
        }

        public void DeleteExistingData()
        {
            ProductRelation.Delete(x => true);
            Product.Delete(x => true);
            Category.Delete(x => true);
        }

        public void CreatePrices()
        {
            var priceGroup = PriceGroup.All().First();
            var products = Product.All();
            var random = new Random();

            foreach (var product in products.Where(x => !x.ProductPrices.Any()))
            {
                product.ProductPrices.Add(new ProductPrice
                {
                    MinimumQuantity = 1,
                    Product = product,
                    Price = new Price
                    {
                        Amount = random.Next(2, 200),
                        PriceGroup = priceGroup
                    }
                });

                product.Save();
            }
        }

        private void CreatePimCategoryDefinition()
        {
            var definition = Definition.SingleOrDefault(d => d.Name == "Default Category Definition");
            var definitionField = DefinitionField.SingleOrDefault(x => x.Definition == definition && x.Name == "PimID");

            if (definitionField == null)
            {
                definitionField = new DefinitionField();
                definitionField.Definition = definition;
                definitionField.Name = "PimID";
                definitionField.DataType = DataType.SingleOrDefault(d => d.TypeName == DataTypes.ShortText);
                definitionField.Save();
            }
        }

        private ProductDefinition CreatePimProductDefinition()
        {
            var productDefinition = CreateProductDefinition("PIM");
            EnsureProductDefinitionField(productDefinition, "PimID", DataTypes.ShortText, false, false);
            EnsureProductDefinitionField(productDefinition, "VariantPimID", DataTypes.ShortText, true, false);
            EnsureProductDefinitionField(productDefinition, "ShowOnHomepage", DataTypes.Boolean, false, false);

            return productDefinition;
        }

        private void CreateEventProductDefinition(ProductDefinition parentDefinition)
        {
            var productDefinition = CreateProductDefinition("Event");
            CreateProductDefinitionRelation(productDefinition, parentDefinition);

            // Variant
            EnsureProductDefinitionField(productDefinition, "EventDate", DataTypes.DateTimePicker, true, false);
            EnsureProductDefinitionField(productDefinition, "EventLocation", DataTypes.ShortText, true, false);
            EnsureProductDefinitionField(productDefinition, "EventLocationDescription", DataTypes.ShortText, true, true);
        }

        private void CreateFoodProductDefinition(ProductDefinition parentDefinition)
        {
            var productDefinition = CreateProductDefinition("Food");
            CreateProductDefinitionRelation(productDefinition, parentDefinition);

            // Product
            EnsureProductDefinitionField(productDefinition, "SalesUnit", DataTypes.ShortText, false, true);
            EnsureProductDefinitionField(productDefinition, "TaxPositionNo", DataTypes.ShortText, false, false);
            EnsureProductDefinitionField(productDefinition, "PackageSize", DataTypes.ShortText, false, false);
            EnsureProductDefinitionField(productDefinition, "PackageWeight", DataTypes.Number, false, false);
            EnsureProductDefinitionField(productDefinition, "Energy", DataTypes.Number, false, false);
            EnsureProductDefinitionField(productDefinition, "Fat", DataTypes.Number, false, false);
        }

        private void CreateWineProductDefinition(ProductDefinition parentDefinition)
        {
            var productDefinition = CreateProductDefinition("Wine");
            CreateProductDefinitionRelation(productDefinition, parentDefinition);

            // Product
            EnsureProductDefinitionField(productDefinition, "SalesUnit", DataTypes.ShortText, false, true);
            EnsureProductDefinitionField(productDefinition, "TaxPositionNo", DataTypes.ShortText, false, false);
            EnsureProductDefinitionField(productDefinition, "PackageSize", DataTypes.ShortText, false, false);
            EnsureProductDefinitionField(productDefinition, "PackageWeight", DataTypes.ShortText, false, false);
            EnsureProductDefinitionField(productDefinition, "LimitedBatch", DataTypes.ShortText, false, false);
            EnsureProductDefinitionField(productDefinition, "Year", DataTypes.ShortText, false, false);
            EnsureProductDefinitionField(productDefinition, "FineWine", DataTypes.Boolean, false, false);
            EnsureProductDefinitionField(productDefinition, "NetContent", DataTypes.ShortText, false, false);
            EnsureProductDefinitionField(productDefinition, "WineManufacturer", DataTypes.ShortText, false, false);
            EnsureProductDefinitionField(productDefinition, "Country", DataTypes.ShortText, false, true);
            EnsureProductDefinitionField(productDefinition, "Region", DataTypes.ShortText, false, true);
            EnsureProductDefinitionField(productDefinition, "District", DataTypes.ShortText, false, true);
            EnsureProductDefinitionField(productDefinition, "WineField", DataTypes.ShortText, false, false);
            EnsureProductDefinitionField(productDefinition, "WineMaker", DataTypes.ShortText, false, true);
            EnsureProductDefinitionField(productDefinition, "SealingSystem", DataTypes.ShortText, false, true);
            EnsureProductDefinitionField(productDefinition, "PackagingType", DataTypes.ShortText, false, true);
            EnsureProductDefinitionField(productDefinition, "Alcohol", DataTypes.Number, false, false);
            EnsureProductDefinitionField(productDefinition, "BottlesProduced", DataTypes.Number, false, false);
            EnsureProductDefinitionField(productDefinition, "ServingTemperature", DataTypes.ShortText, false, false);
            EnsureProductDefinitionField(productDefinition, "PHValue", DataTypes.Number, false, false);
            EnsureProductDefinitionField(productDefinition, "ResidualSugar", DataTypes.Number, false, false);
            EnsureProductDefinitionField(productDefinition, "Acid", DataTypes.Number, false, false);
            EnsureProductDefinitionField(productDefinition, "IsOrganic", DataTypes.Boolean, false, false);
            EnsureProductDefinitionField(productDefinition, "Grapes", DataTypes.ShortText, false, true);
            EnsureProductDefinitionField(productDefinition, "Fat", DataTypes.Number, false, false);
            EnsureProductDefinitionField(productDefinition, "SaturatedFat", DataTypes.Number, false, false);
            EnsureProductDefinitionField(productDefinition, "MonounsaturatedFat", DataTypes.Number, false, false);
            EnsureProductDefinitionField(productDefinition, "VinificationTags", DataTypes.ShortText, false, true);
            EnsureProductDefinitionField(productDefinition, "BottleStorageTags", DataTypes.ShortText, false, true);
            EnsureProductDefinitionField(productDefinition, "CharacteristicaTags", DataTypes.ShortText, false, true);
            EnsureProductDefinitionField(productDefinition, "WellSuitedForTags", DataTypes.ShortText, false, true);
        }

        private void CreateNonFoodProductDefinition(ProductDefinition parentDefinition)
        {
            var productDefinition = CreateProductDefinition("NonFood");
            CreateProductDefinitionRelation(productDefinition, parentDefinition);

            // Product
            EnsureProductDefinitionField(productDefinition, "Brand", DataTypes.ShortText, false, true);
            EnsureProductDefinitionField(productDefinition, "SalesUnit", DataTypes.ShortText, false, true);
            EnsureProductDefinitionField(productDefinition, "TaxPositionNo", DataTypes.ShortText, false, false);
            EnsureProductDefinitionField(productDefinition, "PackageSize", DataTypes.ShortText, false, false);
            EnsureProductDefinitionField(productDefinition, "PackageWeight", DataTypes.Number, false, false);

            // Variant
            EnsureProductDefinitionField(productDefinition, "CPU", DataTypes.ShortText, true, false);
            EnsureProductDefinitionField(productDefinition, "HDD", DataTypes.ShortText, true, false);
            EnsureProductDefinitionField(productDefinition, "Graphics", DataTypes.ShortText, true, true);
            EnsureProductDefinitionField(productDefinition, "NumInPackage", DataTypes.Number, true, false);
            EnsureProductDefinitionField(productDefinition, "Color", DataTypes.ShortText, true, true);
            EnsureProductDefinitionField(productDefinition, "Width", DataTypes.ShortText, true, false);
            EnsureProductDefinitionField(productDefinition, "Length", DataTypes.ShortText, true, false);
            EnsureProductDefinitionField(productDefinition, "InternalMemory", DataTypes.ShortText, true, false);
            EnsureProductDefinitionField(productDefinition, "ClotheSize", DataTypes.ShortText, true, false);
            EnsureProductDefinitionField(productDefinition, "ShoeSize", DataTypes.ShortText, true, false);
            EnsureProductDefinitionField(productDefinition, "PantSize", DataTypes.ShortText, true, false);
        }

        private static ProductDefinition CreateProductDefinition(string name)
        {
            var productDefinition = ProductDefinition.SingleOrDefault(d => d.Name == name) ?? new ProductDefinition();
            productDefinition.Name = name;
            productDefinition.Save();
            return productDefinition;
        }

        private void CreateProductDefinitionRelation(ProductDefinition productDefinition, ProductDefinition parentProductDefinition)
        {
            var relation = ProductDefinitionRelation.SingleOrDefault(x => x.ProductDefinition == productDefinition && x.ParentProductDefinition == parentProductDefinition);
            if (relation == null)
            {
                relation = new ProductDefinitionRelation();
                relation.ProductDefinition = productDefinition;
                relation.ParentProductDefinition = parentProductDefinition;
                relation.Save();
            }
        }

        private void EnsureProductDefinitionField(ProductDefinition definition, string name, string typeName, bool isVariantProperty, bool isMultilingual)
        {
            if (definition.GetDefinitionFields().Any(f => f.Name == name))
                return;

            var field = ProductDefinitionField.SingleOrDefault(f =>
                            f.Name == name && f.ProductDefinition.ProductDefinitionId ==
                            definition.ProductDefinitionId) ??
                        new ProductDefinitionFieldFactory().NewWithDefaults(name);
            field.Name = name;
            field.DataType = DataType.SingleOrDefault(d => d.TypeName == typeName);
            field.Deleted = false;
            field.Multilingual = isMultilingual;
            field.DisplayOnSite = true;
            field.IsVariantProperty = isVariantProperty;
            field.RenderInEditor = true;
            field.Facet = false;

            definition.AddProductDefinitionField(field);
            definition.Save();
        }

        public class DataTypes
        {
            public static string ShortText = "ShortText";
            public static string LongText = "LongText";
            public static string Number = "Number";
            public static string Boolean = "Boolean";
            public static string Image = "Image";
            public static string DatePicker = "DatePicker";
            public static string RichText = "RichText";
            public static string DateTimePicker = "DateTimePicker";
        }
    }
}
