# Struct.PIM.Ucommerce.Connector

The connector can synchronize products, variants and categories from Struct PIM to Ucommerce.

The connector can run as a scheduled job with Struct.PIM.Ucommerce.Connector or be triggered by webhooks with Struct.PIM.Ucommerce.ConnectorAPI.

## Getting started

1. Create your Ucommerce product, variant and catalog definitions.
   - Create a "PIM" definition with "ShortText"-fields named "PimID" and "VariantPimID".
   - Alternative, create demo definitions with "Struct.PIM.Ucommerce.Connector\Installer.cs" using "new Installer().CreateProductDefinitions();".
2. Create mapping code for products, variants and categories.
   - To implement a new mapper, implement one of the interfaces: IProductMapper, IVariantMapper or ICategoryMapper and register the mapper in "Settings.cs".
   - See: "Struct.PIM.Ucommerce.Connector.Integration\StructPim\Mappers\" for examples.
3. Install the "Struct.PIM.Ucommerce.Connector.Extension" NuGet package in Ucommerce.
    - To create and install the NuGet package run "nuget pack StructPimDependencies.nuspec" and install the package from a local package source.
   - The package adds new services to the "StructPim" directory and adds new configurations to Web.config. The new configurations are prefixed with "PimConnector".
   - Beware: if the "Umbraco" directory gets included in the project it may lead to the following error: "The type 'Ucommerce.Web.Shell.Masterpages.MasterPageShell' is ambiguous". To fix the issue, exclude the "Umbraco" directory from the VS project.
4. Update the configuration in App.config/Web.config and "Struct.PIM.Ucommerce.Connector.Integration\Settings.cs".
   - Update database connection-strings.
   - Map Struct PIM structure UUIDs to Ucommerce definition names.
   - Configure languages.
5. Run Struct.PIM.Ucommerce.Connector or Struct.PIM.Ucommerce.ConnectorAPI.

### Demo shop

Download and install Avenue Clothing as an Umbraco package: https://github.com/Ucommercenet/Avenue-Clothing-For-Umbraco/releases
