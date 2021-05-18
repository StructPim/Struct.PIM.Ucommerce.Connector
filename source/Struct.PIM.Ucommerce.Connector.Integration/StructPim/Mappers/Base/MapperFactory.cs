using System;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers.Base
{
    public class MapperFactory
    {
        public static IVariantMapper CreateVariantMapper(Guid productStructureUid)
        {
            var mapperType = Settings.VariantMappers[productStructureUid];
            var mapper = (IVariantMapper)Activator.CreateInstance(mapperType);
            return mapper;
        }
        
        public static IProductMapper CreateProductMapper(Guid pimProductStructureUid)
        {
            if (!Settings.ProductMappers.ContainsKey(pimProductStructureUid))
            {
                return null;
            }

            var mapperType = Settings.ProductMappers[pimProductStructureUid];
            var mapper = (IProductMapper)Activator.CreateInstance(mapperType);
            return mapper;
        }

        public static ICategoryMapper CreateCategoryMapper(Guid catalogueUid)
        {
            var mapperType = Settings.CategoryMappers[catalogueUid];
            var mapper = (ICategoryMapper)Activator.CreateInstance(mapperType);
            return mapper;
        }
    }
}
