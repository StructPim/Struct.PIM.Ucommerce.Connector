using System;
using System.Linq;
using Struct.PIM.Ucommerce.Connector.Integration;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim;
using Struct.PIM.Ucommerce.Connector.Integration.Ucommerce;

namespace Struct.PIM.Ucommerce.Connector
{
    class Program
    {
        static void Main(string[] args)
        {
            var pimBroker = PimBroker.Instance;
            var ucommerceBroker = UcommerceBroker.Instance;
            var statusLogger = new StatusLogger(Settings.StatusLogFilePath);
            var lastUpdate = DateTimeOffset.MinValue; //statusLogger.GetLastUpdate();
            var updateTime = DateTimeOffset.Now;

            // Create or update categories
            var pimCategoryIds = pimBroker.GetUpdatedCategoriesInCatalogue(Settings.Pim.Catalogues.InternalHierarchy, lastUpdate).Select(x => x.Id).ToList();
            var catalogueIntegration = new CatagoryIntegration(pimBroker, ucommerceBroker);
            catalogueIntegration.CreateOrUpdateCategories(pimCategoryIds);
            //catalogueIntegration.DeleteCategories(pimCategoryIds);

            // Create or update products
            var pimProductIds = pimBroker.GetUpdatedProductIds(Settings.Pim.Catalogues.InternalHierarchy, lastUpdate);
            var productIntegration = new ProductIntegration(pimBroker, ucommerceBroker);
            productIntegration.CreateOrUpdateProducts(pimProductIds);
            //productIntegration.DeleteProducts(pimProductIds);

            // Create or update variants
            var pimVariantIds = pimBroker.GetUpdatedVariantIds(Settings.Pim.Catalogues.InternalHierarchy, lastUpdate);
            var variantIntegration = new VariantIntegration(pimBroker, ucommerceBroker);
            variantIntegration.CreateOrUpdateVariants(pimVariantIds);
            //variantIntegration.DeleteVariants(pimVariantIds);

            statusLogger.SetLastModified(updateTime);
        }
    }
}
