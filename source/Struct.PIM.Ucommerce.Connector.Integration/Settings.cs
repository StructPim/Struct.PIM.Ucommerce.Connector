using System;
using System.Collections.Generic;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers;

namespace Struct.PIM.Ucommerce.Connector.Integration
{
    public class Settings
    {
        public static string DefaultCultureCode = CultureCodes[0];
        public static List<string> CultureCodes => new List<string> { "en-GB", "da-DK" };
        public static int BatchSize = 2000;
        public static string StatusLogFilePath = @"c:\temp\pim-integration.txt";

        public class Ucommerce
        {
            public static string DefaultCategoryDefinitionName = "Default Category Definition";
            public static string DefaultProductRelationTypeName = "Default";
            public static string DbConnectionString = "server=struct02;database=struct.pim.ucommerce.demo;user id=struct.pim.ucommerce.demo;password='kAqc2hyGNg8XyYWZ'";
            public static string IndexProductsApiUrl = "https://localhost:44399/Ucommerceapi/search/index";
            public static string IndexCategoriesApiUrl = "https://localhost:44399/Ucommerceapi/search/index";
            public static string IndexingApiKey = "e915987e-630b-422d-ae39-093d0e0b2e62";

            public class Catalogs
            {
                public static string Default = "Demo Store";
            }

            public class ProductDefinitions
            {
                public static string NonFood = "NonFood";
                public static string Wine = "Wine";
                public static string Event = "Event";
                public static string Food = "Food";
            }
        }

        public class Pim
        {
            public static string Url = "https://api.v37.cloud1.structpim.com/";
            public static string ApiKey = "7164da70-1c54-46ea-8a75-52f243c0ac66";
            public static string DefaultSegment = "Desktop";

            public class Catalogues
            {
                public static Guid InternalHierarchy = Guid.Parse("4ce9b67b-8632-43bb-8ca8-a71d575353a9");
            }

            public class ProductStructures
            {
                public static Guid NonFood = Guid.Parse("df5733d0-af60-4299-9449-1bed2f4f9ba1");
                public static Guid Wine = Guid.Parse("e64b2da8-8041-4be4-a1c1-1e87e8c87341");
                public static Guid Event = Guid.Parse("472d0ed6-fbb9-494d-9b57-4e7fc6bc2d36");
                public static Guid Food = Guid.Parse("ade5dac1-9862-45db-b9c2-d5c2b3e0dd40");
            }
        }

        public static Dictionary<Guid, string> PimCatalogueToUcCatalogMap = new Dictionary<Guid, string>
        {
            { Pim.Catalogues.InternalHierarchy, Ucommerce.Catalogs.Default },
        };

        public static Dictionary<Guid, string> PimProductStructureToUcProductDefinitionMap = new Dictionary<Guid, string>
        {
            { Pim.ProductStructures.NonFood, Ucommerce.ProductDefinitions.NonFood },
            { Pim.ProductStructures.Wine, Ucommerce.ProductDefinitions.Wine },
            { Pim.ProductStructures.Event, Ucommerce.ProductDefinitions.Event },
            { Pim.ProductStructures.Food, Ucommerce.ProductDefinitions.Food },
        };

        public static Dictionary<Guid, Type> ProductMappers = new Dictionary<Guid, Type>
        {
            { Pim.ProductStructures.NonFood, typeof(NonFoodProductMapper) },
            { Pim.ProductStructures.Wine, typeof(WineProductMapper) },
            { Pim.ProductStructures.Event, typeof(EventProductMapper) },
            { Pim.ProductStructures.Food, typeof(FoodProductMapper) },
        };

        public static Dictionary<Guid, Type> VariantMappers = new Dictionary<Guid, Type>
        {
            { Pim.ProductStructures.NonFood, typeof(NonFoodVariantMapper) },
            { Pim.ProductStructures.Event, typeof(EventVariantMapper) },
        };

        public static Dictionary<Guid, Type> CategoryMappers = new Dictionary<Guid, Type>
        {
            { Pim.Catalogues.InternalHierarchy, typeof(InternalHierarchyCategoryMapper) },
        };
    }
}
