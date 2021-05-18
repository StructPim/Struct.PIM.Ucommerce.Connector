using System;
using System.Collections.Generic;
using Struct.PIM.Api.Models.Shared;
using Struct.PIM.Api.Models.Attribute;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models
{
    #region ProductModels
    public partial class NonfoodProductModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
        /// <summary>
        /// Select the sales channels this product shall be published to
        /// </summary>
        public virtual List<SalesChannelsGlobalListModel> SalesChannels { get; set; }
        /// <summary>
        /// Brand
        /// </summary>
        public virtual BrandsGlobalListModel Brand { get; set; }
        /// <summary>
        /// Select the unit that sale quantity is measued in
        /// </summary>
        public virtual SalesUnitsGlobalListModel SalesUnit { get; set; }
        /// <summary>
        /// Select the appropriate accounting group
        /// </summary>
        public virtual AccountingGroupsGlobalListModel AccountingGroup { get; set; }
        /// <summary>
        /// Model nr
        /// </summary>
        public virtual string ModelNumber { get; set; }
        /// <summary>
        /// Add the tax position no.
        /// </summary>
        public virtual string TaxPositionNo { get; set; }
        /// <summary>
        /// Select if this product is labelled as dangerous goods, thus requiring special attention during transport
        /// </summary>
        public virtual bool? DangerousGoods { get; set; }
        /// <summary>
        /// Delivery group
        /// </summary>
        public virtual DeliveryGroupsGlobalListModel DeliveryGroup { get; set; }
        /// <summary>
        /// Set the gross size of the product (width x length x height)
        /// </summary>
        public virtual PackageSizeModel PackageSize { get; set; }
        /// <summary>
        /// Enter the gross weight of the product
        /// </summary>
        public virtual decimal? PackageWeight { get; set; }
        /// <summary>
        /// Add a short description of this product which can be used as a teaser.
        /// </summary>
        public virtual List<SegmentedLocalizedData<string>> ShortDescription { get; set; }
        /// <summary>
        /// Add a complete description of the product. Do not include technical specifications here. This should only be describing text
        /// </summary>
        public virtual List<SegmentedLocalizedData<string>> LongDescription { get; set; }
        /// <summary>
        /// Add a specific description for the automatic catalogue production
        /// </summary>
        public virtual List<LocalizedData<string>> CatalogueDescription { get; set; }
        /// <summary>
        /// Select a highlighting for your product
        /// </summary>
        public virtual ProductHighlightsGlobalListModel Highlight { get; set; }
        /// <summary>
        /// The primary image is the default image for this product and is shown as a representative for the product whenever the product is displayed
        /// </summary>
        public virtual string PrimaryImage { get; set; }
        /// <summary>
        /// Add extra images of the product seen from different angles or in different use cases
        /// </summary>
        public virtual List<string> ExtraImages { get; set; }
        /// <summary>
        /// Add videos from Youtube or Vimeo
        /// </summary>
        public virtual List<VideosModel> Videos { get; set; }
        /// <summary>
        /// Documents
        /// </summary>
        public virtual List<DocumentsModel> Documents { get; set; }
        /// <summary>
        /// Select accessories for this product
        /// </summary>
        public virtual List<int> Accessories { get; set; }
        /// <summary>
        /// Select complimentary products which are are considered more premium than this product
        /// </summary>
        public virtual List<int> Upsale { get; set; }
        /// <summary>
        /// Select products from other categories which would make sense to buy together with this product
        /// </summary>
        public virtual List<int> CrossSale { get; set; }
        /// <summary>
        /// Select complimentary products which a customer may consider as an alternative to this product
        /// </summary>
        public virtual List<int> Complimentary { get; set; }
    }

    public partial class WineProductModel
    {
        /// <summary>
        /// SKU
        /// </summary>
        public virtual string SKU { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
        /// <summary>
        /// Select the sales channels this product shall be published to
        /// </summary>
        public virtual List<SalesChannelsGlobalListModel> SalesChannels { get; set; }
        /// <summary>
        /// Select the unit that sale quantity is measued in
        /// </summary>
        public virtual SalesUnitsGlobalListModel SalesUnit { get; set; }
        /// <summary>
        /// Select the appropriate accounting group
        /// </summary>
        public virtual AccountingGroupsGlobalListModel AccountingGroup { get; set; }
        /// <summary>
        /// Add the tax position no.
        /// </summary>
        public virtual string TaxPositionNo { get; set; }
        /// <summary>
        /// Delivery group
        /// </summary>
        public virtual DeliveryGroupsGlobalListModel DeliveryGroup { get; set; }
        /// <summary>
        /// Set the gross size of the product (width x length x height)
        /// </summary>
        public virtual PackageSizeModel PackageSize { get; set; }
        /// <summary>
        /// Enter the gross weight of the product
        /// </summary>
        public virtual decimal? PackageWeight { get; set; }
        /// <summary>
        /// Select if this product only exist in a limited amount and thus may not be restocked once sold oud
        /// </summary>
        public virtual bool? LimitedBatch { get; set; }
        /// <summary>
        /// Add the year for this wine. Leave empty, if it is unknown
        /// </summary>
        public virtual decimal? Year { get; set; }
        /// <summary>
        /// Select if this wine is categorized as a "Fine wine"
        /// </summary>
        public virtual bool? FineWine { get; set; }
        /// <summary>
        /// Set the net wine content of this product
        /// </summary>
        public virtual NetContentModel NetContent { get; set; }
        /// <summary>
        /// Manufacturer
        /// </summary>
        public virtual WineManufacturersGlobalListModel WineManufacturer { get; set; }
        /// <summary>
        /// Originating country of this wine
        /// </summary>
        public virtual CountriesGlobalListModel Country { get; set; }
        /// <summary>
        /// Select the region this wine is produced in
        /// </summary>
        public virtual WineRegionsGlobalListModel Region { get; set; }
        /// <summary>
        /// Select the "Appellation d'Origine Protégée" of this wine
        /// </summary>
        public virtual WineDistrictsGlobalListModel District { get; set; }
        /// <summary>
        /// If known, enter the field the grapes originate from
        /// </summary>
        public virtual WineFieldsGlobalListModel WineField { get; set; }
        /// <summary>
        /// Wine maker
        /// </summary>
        public virtual WineMakersGlobalListModel WineMaker { get; set; }
        /// <summary>
        /// Sealing system
        /// </summary>
        public virtual SealingSystemsGlobalListModel SealingSystem { get; set; }
        /// <summary>
        /// Packaging type
        /// </summary>
        public virtual BottleTypesGlobalListModel PackagingType { get; set; }
        /// <summary>
        /// Set the amount of alcohol in this wine
        /// </summary>
        public virtual decimal? Alcohol { get; set; }
        /// <summary>
        /// If known, set the total number of bottles produced
        /// </summary>
        public virtual decimal? BottlesProduced { get; set; }
        /// <summary>
        /// Serving temperature
        /// </summary>
        public virtual ServingTemperatureModel ServingTemperature { get; set; }
        /// <summary>
        /// pH-value
        /// </summary>
        public virtual decimal? PHValue { get; set; }
        /// <summary>
        /// Residual sugar
        /// </summary>
        public virtual decimal? ResidualSugar { get; set; }
        /// <summary>
        /// Acid content
        /// </summary>
        public virtual decimal? Acid { get; set; }
        /// <summary>
        /// Select if this wine is organic
        /// </summary>
        public virtual bool? IsOrganic { get; set; }
        /// <summary>
        /// Select the grapes used for producing this wine as well as the amount of each grape used
        /// </summary>
        public virtual List<GrapesModel> Grapes { get; set; }
        /// <summary>
        /// Set the total energy content of 100 g of the product
        /// </summary>
        public virtual decimal? Energy { get; set; }
        /// <summary>
        /// Set the amount of fat in the product
        /// </summary>
        public virtual decimal? Fat { get; set; }
        /// <summary>
        /// Set how much of the fat is saturated
        /// </summary>
        public virtual decimal? SaturatedFat { get; set; }
        /// <summary>
        /// Set the amount of the fat, which is monounsaturated
        /// </summary>
        public virtual decimal? MonounsaturatedFat { get; set; }
        /// <summary>
        /// Set the amount of fat, which is polyunsaturated
        /// </summary>
        public virtual decimal? PolyunsaturatedFat { get; set; }
        /// <summary>
        /// Set the amount of cabohydrates per 100g
        /// </summary>
        public virtual decimal? Carbohydrates { get; set; }
        /// <summary>
        /// Set the amount of sugars per 100g
        /// </summary>
        public virtual decimal? Sugars { get; set; }
        /// <summary>
        /// Set the amount of polyols per 100g
        /// </summary>
        public virtual decimal? Polyols { get; set; }
        /// <summary>
        /// Set the amount of starch per 100g
        /// </summary>
        public virtual decimal? Starch { get; set; }
        /// <summary>
        /// Fibers
        /// </summary>
        public virtual decimal? Fibers { get; set; }
        /// <summary>
        /// Set the amount of protein per 100g
        /// </summary>
        public virtual decimal? Protein { get; set; }
        /// <summary>
        /// Set the amount of salt per 100g
        /// </summary>
        public virtual decimal? Salt { get; set; }
        /// <summary>
        /// Select if this wine contains sulfites
        /// </summary>
        public virtual bool? ContainsSulfites { get; set; }
        /// <summary>
        /// Grape tags
        /// </summary>
        public virtual List<List<LocalizedData<string>>> GrapeTags { get; set; }
        /// <summary>
        /// Vinification tags
        /// </summary>
        public virtual List<List<LocalizedData<string>>> VinificationTags { get; set; }
        /// <summary>
        /// Bottle storage tags
        /// </summary>
        public virtual List<List<LocalizedData<string>>> BottleStorageTags { get; set; }
        /// <summary>
        /// Characteristica tags
        /// </summary>
        public virtual List<List<LocalizedData<string>>> CharacteristicaTags { get; set; }
        /// <summary>
        /// Well suited for tags
        /// </summary>
        public virtual List<List<LocalizedData<string>>> WellSuitedForTags { get; set; }
        /// <summary>
        /// Add a short description of this product which can be used as a teaser.
        /// </summary>
        public virtual List<SegmentedLocalizedData<string>> ShortDescription { get; set; }
        /// <summary>
        /// Add a complete description of the product. Do not include technical specifications here. This should only be describing text
        /// </summary>
        public virtual List<SegmentedLocalizedData<string>> LongDescription { get; set; }
        /// <summary>
        /// Add a specific description for the automatic catalogue production
        /// </summary>
        public virtual List<LocalizedData<string>> CatalogueDescription { get; set; }
        /// <summary>
        /// Select a highlighting for your product
        /// </summary>
        public virtual ProductHighlightsGlobalListModel Highlight { get; set; }
        /// <summary>
        /// The primary image is the default image for this product and is shown as a representative for the product whenever the product is displayed
        /// </summary>
        public virtual string PrimaryImage { get; set; }
        /// <summary>
        /// Add extra images of the product seen from different angles or in different use cases
        /// </summary>
        public virtual List<string> ExtraImages { get; set; }
        /// <summary>
        /// Add videos from Youtube or Vimeo
        /// </summary>
        public virtual List<VideosModel> Videos { get; set; }
        /// <summary>
        /// Select accessories for this product
        /// </summary>
        public virtual List<int> Accessories { get; set; }
        /// <summary>
        /// Select complimentary products which are are considered more premium than this product
        /// </summary>
        public virtual List<int> Upsale { get; set; }
        /// <summary>
        /// Select products from other categories which would make sense to buy together with this product
        /// </summary>
        public virtual List<int> CrossSale { get; set; }
        /// <summary>
        /// Select complimentary products which a customer may consider as an alternative to this product
        /// </summary>
        public virtual List<int> Complimentary { get; set; }
    }

    public partial class EventProductModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
        /// <summary>
        /// Select the sales channels this product shall be published to
        /// </summary>
        public virtual List<SalesChannelsGlobalListModel> SalesChannels { get; set; }
        /// <summary>
        /// Add a short description of this product which can be used as a teaser.
        /// </summary>
        public virtual List<SegmentedLocalizedData<string>> ShortDescription { get; set; }
        /// <summary>
        /// Add a complete description of the product. Do not include technical specifications here. This should only be describing text
        /// </summary>
        public virtual List<SegmentedLocalizedData<string>> LongDescription { get; set; }
        /// <summary>
        /// Select a highlighting for your product
        /// </summary>
        public virtual ProductHighlightsGlobalListModel Highlight { get; set; }
        /// <summary>
        /// The primary image is the default image for this product and is shown as a representative for the product whenever the product is displayed
        /// </summary>
        public virtual string PrimaryImage { get; set; }
        /// <summary>
        /// Add extra images of the product seen from different angles or in different use cases
        /// </summary>
        public virtual List<string> ExtraImages { get; set; }
        /// <summary>
        /// Add videos from Youtube or Vimeo
        /// </summary>
        public virtual List<VideosModel> Videos { get; set; }
        /// <summary>
        /// Documents
        /// </summary>
        public virtual List<DocumentsModel> Documents { get; set; }
    }

    public partial class FoodProductModel
    {
        /// <summary>
        /// SKU
        /// </summary>
        public virtual string SKU { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
        /// <summary>
        /// Select the sales channels this product shall be published to
        /// </summary>
        public virtual List<SalesChannelsGlobalListModel> SalesChannels { get; set; }
        /// <summary>
        /// Select the unit that sale quantity is measued in
        /// </summary>
        public virtual SalesUnitsGlobalListModel SalesUnit { get; set; }
        /// <summary>
        /// Select the appropriate accounting group
        /// </summary>
        public virtual AccountingGroupsGlobalListModel AccountingGroup { get; set; }
        /// <summary>
        /// Add the tax position no.
        /// </summary>
        public virtual string TaxPositionNo { get; set; }
        /// <summary>
        /// Delivery group
        /// </summary>
        public virtual DeliveryGroupsGlobalListModel DeliveryGroup { get; set; }
        /// <summary>
        /// Set the gross size of the product (width x length x height)
        /// </summary>
        public virtual PackageSizeModel PackageSize { get; set; }
        /// <summary>
        /// Enter the gross weight of the product
        /// </summary>
        public virtual decimal? PackageWeight { get; set; }
        /// <summary>
        /// Set the total energy content of 100 g of the product
        /// </summary>
        public virtual decimal? Energy { get; set; }
        /// <summary>
        /// Set the amount of fat in the product
        /// </summary>
        public virtual decimal? Fat { get; set; }
        /// <summary>
        /// Set how much of the fat is saturated
        /// </summary>
        public virtual decimal? SaturatedFat { get; set; }
        /// <summary>
        /// Set the amount of the fat, which is monounsaturated
        /// </summary>
        public virtual decimal? MonounsaturatedFat { get; set; }
        /// <summary>
        /// Set the amount of fat, which is polyunsaturated
        /// </summary>
        public virtual decimal? PolyunsaturatedFat { get; set; }
        /// <summary>
        /// Set the amount of cabohydrates per 100g
        /// </summary>
        public virtual decimal? Carbohydrates { get; set; }
        /// <summary>
        /// Set the amount of sugars per 100g
        /// </summary>
        public virtual decimal? Sugars { get; set; }
        /// <summary>
        /// Set the amount of polyols per 100g
        /// </summary>
        public virtual decimal? Polyols { get; set; }
        /// <summary>
        /// Set the amount of starch per 100g
        /// </summary>
        public virtual decimal? Starch { get; set; }
        /// <summary>
        /// Fibers
        /// </summary>
        public virtual decimal? Fibers { get; set; }
        /// <summary>
        /// Set the amount of protein per 100g
        /// </summary>
        public virtual decimal? Protein { get; set; }
        /// <summary>
        /// Set the amount of salt per 100g
        /// </summary>
        public virtual decimal? Salt { get; set; }
        /// <summary>
        /// Add the ingredients of this product
        /// </summary>
        public virtual List<IngredientsModel> Ingredients { get; set; }
        /// <summary>
        /// Add a short description of this product which can be used as a teaser.
        /// </summary>
        public virtual List<SegmentedLocalizedData<string>> ShortDescription { get; set; }
        /// <summary>
        /// Add a complete description of the product. Do not include technical specifications here. This should only be describing text
        /// </summary>
        public virtual List<SegmentedLocalizedData<string>> LongDescription { get; set; }
        /// <summary>
        /// Add a specific description for the automatic catalogue production
        /// </summary>
        public virtual List<LocalizedData<string>> CatalogueDescription { get; set; }
        /// <summary>
        /// Select a highlighting for your product
        /// </summary>
        public virtual ProductHighlightsGlobalListModel Highlight { get; set; }
        /// <summary>
        /// The primary image is the default image for this product and is shown as a representative for the product whenever the product is displayed
        /// </summary>
        public virtual string PrimaryImage { get; set; }
        /// <summary>
        /// Add extra images of the product seen from different angles or in different use cases
        /// </summary>
        public virtual List<string> ExtraImages { get; set; }
        /// <summary>
        /// Add videos from Youtube or Vimeo
        /// </summary>
        public virtual List<VideosModel> Videos { get; set; }
        /// <summary>
        /// Select accessories for this product
        /// </summary>
        public virtual List<int> Accessories { get; set; }
        /// <summary>
        /// Select complimentary products which are are considered more premium than this product
        /// </summary>
        public virtual List<int> Upsale { get; set; }
        /// <summary>
        /// Select products from other categories which would make sense to buy together with this product
        /// </summary>
        public virtual List<int> CrossSale { get; set; }
        /// <summary>
        /// Select complimentary products which a customer may consider as an alternative to this product
        /// </summary>
        public virtual List<int> Complimentary { get; set; }
    }

    #endregion

    #region VariantModels
    public partial class NonfoodVariantModel
    {
        /// <summary>
        /// SKU
        /// </summary>
        public virtual string SKU { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
        /// <summary>
        /// Suppliers
        /// </summary>
        public virtual List<SuppliersModel> Suppliers { get; set; }
        /// <summary>
        /// CPU
        /// </summary>
        public virtual CPUModelsGlobalListModel CPU { get; set; }
        /// <summary>
        /// Harddisk size
        /// </summary>
        public virtual HDDModel HDD { get; set; }
        /// <summary>
        /// Graphics card (GPU)
        /// </summary>
        public virtual GraphicsModelsGlobalListModel Graphics { get; set; }
        /// <summary>
        /// Package size
        /// </summary>
        public virtual decimal? NumInPackage { get; set; }
        /// <summary>
        /// Color
        /// </summary>
        public virtual ColorsGlobalListModel Color { get; set; }
        /// <summary>
        /// Width
        /// </summary>
        public virtual WidthModel Width { get; set; }
        /// <summary>
        /// Length
        /// </summary>
        public virtual LengthModel Length { get; set; }
        /// <summary>
        /// Internal memory
        /// </summary>
        public virtual InternalMemoryModel InternalMemory { get; set; }
        /// <summary>
        /// Clothe size
        /// </summary>
        public virtual ClotheSizesGlobalListModel ClotheSize { get; set; }
        /// <summary>
        /// Shoe size
        /// </summary>
        public virtual ShoeSizesGlobalListModel ShoeSize { get; set; }
        /// <summary>
        /// Size
        /// </summary>
        public virtual PantSizesGlobalListModel PantSize { get; set; }
        /// <summary>
        /// Set the gross size of the product (width x length x height)
        /// </summary>
        public virtual PackageSizeModel PackageSize { get; set; }
        /// <summary>
        /// Enter the gross weight of the product
        /// </summary>
        public virtual decimal? PackageWeight { get; set; }
        /// <summary>
        /// The primary image is the default image for this product and is shown as a representative for the product whenever the product is displayed
        /// </summary>
        public virtual string PrimaryImage { get; set; }
        /// <summary>
        /// Add extra images of the product seen from different angles or in different use cases
        /// </summary>
        public virtual List<string> ExtraImages { get; set; }
    }

    public partial class EventVariantModel
    {
        /// <summary>
        /// SKU
        /// </summary>
        public virtual string SKU { get; set; }
        /// <summary>
        /// Event date
        /// </summary>
        public virtual DateTimeOffset? EventDate { get; set; }
        /// <summary>
        /// Event location
        /// </summary>
        public virtual string EventLocation { get; set; }
        /// <summary>
        /// Event location description
        /// </summary>
        public virtual List<LocalizedData<string>> EventLocationDescription { get; set; }
    }

    #endregion

    #region CategoryModels
    public partial class ElectronicssiteCategoryModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> CategoryName { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public virtual List<LocalizedData<string>> CategoryDescription { get; set; }
        /// <summary>
        /// Setup filters to use for this category in your E-commerce solution
        /// </summary>
        public virtual List<FilterSetupModel> FilterSetup { get; set; }
    }

    public partial class InternalhierarchyCategoryModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> CategoryName { get; set; }
        /// <summary>
        /// Select attributes that shall be displayed as featured on products in this category
        /// </summary>
        public virtual List<AttributeReference> FeaturedSpecs { get; set; }
        /// <summary>
        /// Select an image to represent this category
        /// </summary>
        public virtual string CategoryImage { get; set; }
        /// <summary>
        /// Setup filters to use for this category in your E-commerce solution
        /// </summary>
        public virtual List<FilterSetupModel> FilterSetup { get; set; }
    }

    public partial class WineChannelCategoryModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> CategoryName { get; set; }
    }

    #endregion

    #region Attributes
    public partial class PackageSizeModel
    {
        /// <summary>
        /// Width
        /// </summary>
        public virtual decimal? Width { get; set; }
        /// <summary>
        /// Length
        /// </summary>
        public virtual decimal? Length { get; set; }
        /// <summary>
        /// Height
        /// </summary>
        public virtual decimal? Height { get; set; }
    }

    public partial class NetContentModel
    {
        /// <summary>
        /// Content amount
        /// </summary>
        public virtual decimal? ContentAmount { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        public virtual VolumeUnitsGlobalListModel Unit { get; set; }
    }

    public partial class ServingTemperatureModel
    {
        /// <summary>
        /// Min
        /// </summary>
        public virtual decimal? Min { get; set; }
        /// <summary>
        /// Max
        /// </summary>
        public virtual decimal? Max { get; set; }
    }

    public partial class HDDModel
    {
        /// <summary>
        /// Size
        /// </summary>
        public virtual decimal? Size { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        public virtual string Unit { get; set; }
    }

    public partial class WidthModel
    {
        /// <summary>
        /// Width
        /// </summary>
        public virtual decimal? Width { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        public virtual LengthUnitsGlobalListModel Unit { get; set; }
    }

    public partial class LengthModel
    {
        /// <summary>
        /// Length
        /// </summary>
        public virtual decimal? Length { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        public virtual LengthUnitsGlobalListModel Unit { get; set; }
    }

    public partial class InternalMemoryModel
    {
        /// <summary>
        /// Memory
        /// </summary>
        public virtual decimal? Memory { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        public virtual string Unit { get; set; }
    }

    public partial class USB30Model
    {
        /// <summary>
        /// Has USB 3.0 ports
        /// </summary>
        public virtual bool? HasPorts { get; set; }
        /// <summary>
        /// Number of ports
        /// </summary>
        public virtual decimal? NumberOfPorts { get; set; }
    }

    public partial class DiameterModel
    {
        /// <summary>
        /// Diameter
        /// </summary>
        public virtual decimal? Diameter { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        public virtual LengthUnitsGlobalListModel Unit { get; set; }
    }

    public partial class HDMIConnectionModel
    {
        /// <summary>
        /// Has HDMI port
        /// </summary>
        public virtual bool? HasPorts { get; set; }
        /// <summary>
        /// Number of ports
        /// </summary>
        public virtual decimal? NumberOfPorts { get; set; }
    }

    public partial class ResolutionInBlackWhiteModel
    {
        /// <summary>
        /// x
        /// </summary>
        public virtual decimal? x { get; set; }
        /// <summary>
        /// y
        /// </summary>
        public virtual decimal? y { get; set; }
    }

    public partial class DistanceBetweenMountingHolesModel
    {
        /// <summary>
        /// Horizontal
        /// </summary>
        public virtual decimal? Horizontal { get; set; }
        /// <summary>
        /// Vertical
        /// </summary>
        public virtual decimal? Vertical { get; set; }
    }

    public partial class FlexibleWidthModel
    {
        /// <summary>
        /// Width
        /// </summary>
        public virtual decimal? Width { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        public virtual LengthUnitsGlobalListModel Unit { get; set; }
    }

    public partial class USBCModel
    {
        /// <summary>
        /// Has USB-C ports
        /// </summary>
        public virtual bool? HasPorts { get; set; }
        /// <summary>
        /// Number of ports
        /// </summary>
        public virtual decimal? NumberOfPorts { get; set; }
    }

    public partial class ResolutionInColorModel
    {
        /// <summary>
        /// x
        /// </summary>
        public virtual decimal? x { get; set; }
        /// <summary>
        /// y
        /// </summary>
        public virtual decimal? y { get; set; }
    }

    public partial class FlexibleWeightModel
    {
        /// <summary>
        /// Weight
        /// </summary>
        public virtual decimal? Weight { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        public virtual WeightUnitsGlobalListModel Unit { get; set; }
    }

    public partial class TotalStorageCapacityModel
    {
        /// <summary>
        /// Size
        /// </summary>
        public virtual decimal? Size { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        public virtual string Unit { get; set; }
    }

    public partial class LengthSpecModel
    {
        /// <summary>
        /// Length
        /// </summary>
        public virtual decimal? Length { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        public virtual LengthUnitsGlobalListModel Unit { get; set; }
    }

    public partial class MaxResolutionModel
    {
        /// <summary>
        /// x
        /// </summary>
        public virtual decimal? x { get; set; }
        /// <summary>
        /// y
        /// </summary>
        public virtual decimal? y { get; set; }
    }

    public partial class StorageCapacity1Model
    {
        /// <summary>
        /// Size
        /// </summary>
        public virtual decimal? Size { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        public virtual string Unit { get; set; }
    }

    public partial class USB20Model
    {
        /// <summary>
        /// Has USB 2.0 ports
        /// </summary>
        public virtual bool? HasPorts { get; set; }
        /// <summary>
        /// Number of ports
        /// </summary>
        public virtual decimal? NumberOfPorts { get; set; }
    }

    public partial class FlexibleThicknessModel
    {
        /// <summary>
        /// Thickness
        /// </summary>
        public virtual decimal? Thickness { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        public virtual LengthUnitsGlobalListModel Unit { get; set; }
    }

    public partial class ScreenResolutionModel
    {
        /// <summary>
        /// x
        /// </summary>
        public virtual decimal? x { get; set; }
        /// <summary>
        /// y
        /// </summary>
        public virtual decimal? y { get; set; }
    }


    public partial class VideosModel
    {
        /// <summary>
        /// Insert the unique id of the video at Youtube or Vimeo
        /// </summary>
        public virtual string VideoId { get; set; }
        /// <summary>
        /// Select the streaming source of this video
        /// </summary>
        public virtual VideoSourcesGlobalListModel VideoSource { get; set; }
    }

    public partial class DocumentsModel
    {
        /// <summary>
        /// Select the document from the media archive
        /// </summary>
        public virtual string File { get; set; }
        /// <summary>
        /// Select the type document
        /// </summary>
        public virtual DocumentTypesGlobalListModel DocumentType { get; set; }
        /// <summary>
        /// Select when this document is valid from. Empty means no start date
        /// </summary>
        public virtual DateTimeOffset? ValidFrom { get; set; }
        /// <summary>
        /// Select when this document is valid to. Empty means unlimited
        /// </summary>
        public virtual DateTimeOffset? ValidTo { get; set; }
    }

    public partial class GrapesModel
    {
        /// <summary>
        /// Grape
        /// </summary>
        public virtual GrapesGlobalListModel Grape { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        public virtual decimal? Content { get; set; }
    }

    public partial class IngredientsModel
    {
        /// <summary>
        /// Ingredient
        /// </summary>
        public virtual IngredientsGlobalListModel Ingredient { get; set; }
        /// <summary>
        /// Sub ingredient
        /// </summary>
        public virtual List<IngredientsGlobalListModel> SubIngredient { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        public virtual decimal? Content { get; set; }
        /// <summary>
        /// Organic
        /// </summary>
        public virtual bool? Organic { get; set; }
    }

    public partial class SuppliersModel
    {
        /// <summary>
        /// Select suppliers and corresponding supplier product no.
        /// </summary>
        public virtual SuppliersGlobalListModel Supplier { get; set; }
        /// <summary>
        /// Supplier product no.
        /// </summary>
        public virtual string SupplierProductNo { get; set; }
    }

    public partial class FilterSetupModel
    {
        /// <summary>
        /// Display name
        /// </summary>
        public virtual List<LocalizedData<string>> DisplayName { get; set; }
        /// <summary>
        /// Filter
        /// </summary>
        public virtual AttributeReference FilterSelector { get; set; }
        /// <summary>
        /// Select how you want the filter do be displayed
        /// </summary>
        public virtual FilterTypesGlobalListModel FilterType { get; set; }
    }

    public partial class MaterialsModel
    {
        /// <summary>
        /// Material
        /// </summary>
        public virtual List<LocalizedData<string>> Material { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        public virtual decimal? Content { get; set; }
    }

    #endregion

    #region GlobalListModels
    public partial class BaseColorsGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
        /// <summary>
        /// Hex Color
        /// </summary>
        public virtual string HexColor { get; set; }
    }

    public partial class ColorsGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
        /// <summary>
        /// Base color
        /// </summary>
        public virtual BaseColorsGlobalListModel BaseColor { get; set; }
        /// <summary>
        /// Hex Color
        /// </summary>
        public virtual string HexColor { get; set; }
    }

    public partial class BrandsGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Logo
        /// </summary>
        public virtual string Logo { get; set; }
        /// <summary>
        /// Add a story about this brand
        /// </summary>
        public virtual List<LocalizedData<string>> Description { get; set; }
    }

    public partial class ProductLifecyclesGlobalListModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual string Id { get; set; }
        /// <summary>
        /// Text
        /// </summary>
        public virtual string Text { get; set; }
    }

    public partial class SuppliersGlobalListModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual string Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
    }

    public partial class SalesUnitsGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
    }

    public partial class DeliveryGroupsGlobalListModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual string Id { get; set; }
        /// <summary>
        /// Text
        /// </summary>
        public virtual string Text { get; set; }
    }

    public partial class AccountingGroupsGlobalListModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual string Id { get; set; }
        /// <summary>
        /// Text
        /// </summary>
        public virtual string Text { get; set; }
    }

    public partial class VideoSourcesGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
    }

    public partial class WeightUnitsGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
    }

    public partial class DocumentTypesGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
    }

    public partial class LengthUnitsGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
    }

    public partial class ProductTypesGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
    }

    public partial class CPUManufacturersGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Logo
        /// </summary>
        public virtual string Logo { get; set; }
    }

    public partial class ProcessorTypesGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
    }

    public partial class CPUModelsGlobalListModel
    {
        /// <summary>
        /// CPU Manufacturer
        /// </summary>
        public virtual CPUManufacturersGlobalListModel CPUManufacturer { get; set; }
        /// <summary>
        /// Processor type
        /// </summary>
        public virtual ProcessorTypesGlobalListModel ProcessorType { get; set; }
        /// <summary>
        /// Model no.
        /// </summary>
        public virtual string ModelNumber { get; set; }
        /// <summary>
        /// CPU Generation
        /// </summary>
        public virtual string CPUGeneration { get; set; }
        /// <summary>
        /// CPU Generation name
        /// </summary>
        public virtual string CPUGenerationName { get; set; }
        /// <summary>
        /// Number of cores
        /// </summary>
        public virtual decimal? NumberOfCores { get; set; }
        /// <summary>
        /// Clock frequency
        /// </summary>
        public virtual decimal? ClockFrequency { get; set; }
        /// <summary>
        /// CPU cache
        /// </summary>
        public virtual decimal? CPUCache { get; set; }
    }

    public partial class RAMTypesGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
    }

    public partial class StorageTypesGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
    }

    public partial class GraphicsManufacturersGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Logo
        /// </summary>
        public virtual string Logo { get; set; }
    }

    public partial class GraphicsModelsGlobalListModel
    {
        /// <summary>
        /// Manufacturer
        /// </summary>
        public virtual GraphicsManufacturersGlobalListModel Manufacturer { get; set; }
        /// <summary>
        /// Model no.
        /// </summary>
        public virtual string ModelNumber { get; set; }
        /// <summary>
        /// Clock frequency
        /// </summary>
        public virtual decimal? ClockFrequency { get; set; }
    }

    public partial class ScreenTypesGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public virtual List<LocalizedData<string>> Description { get; set; }
    }

    public partial class ResolutionStandardsGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
    }

    public partial class BatteryTypesGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public virtual List<LocalizedData<string>> Description { get; set; }
    }

    public partial class OperatingSystemsGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }
    }

    public partial class MaterialsGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
    }

    public partial class NotchTypesGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
    }

    public partial class ToolTypesGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
    }

    public partial class CountriesGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
    }

    public partial class VolumeUnitsGlobalListModel
    {
        /// <summary>
        /// Unit
        /// </summary>
        public virtual string Unit { get; set; }
        /// <summary>
        /// Enter the factor of this unit that corresponds to 1 l
        /// </summary>
        public virtual decimal? NormalizationFactor { get; set; }
    }

    public partial class WineManufacturersGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
        /// <summary>
        /// Link
        /// </summary>
        public virtual string Link { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        public virtual string Address { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public virtual List<LocalizedData<string>> Description { get; set; }
    }

    public partial class WineFieldsGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
    }

    public partial class WineRegionsGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public virtual List<LocalizedData<string>> Description { get; set; }
    }

    public partial class WineDistrictsGlobalListModel
    {
        /// <summary>
        /// Region
        /// </summary>
        public virtual WineRegionsGlobalListModel Region { get; set; }
        /// <summary>
        /// AOP code
        /// </summary>
        public virtual string AOPCode { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public virtual List<LocalizedData<string>> Description { get; set; }
        /// <summary>
        /// Map of the district
        /// </summary>
        public virtual string Map { get; set; }
    }

    public partial class GrapesGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public virtual List<LocalizedData<string>> Description { get; set; }
    }

    public partial class BottleTypesGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
    }

    public partial class SealingSystemsGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
    }

    public partial class WineMakersGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
    }

    public partial class IngredientsGlobalListModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public virtual List<LocalizedData<string>> Name { get; set; }
        /// <summary>
        /// Allergene
        /// </summary>
        public virtual bool? Allergene { get; set; }
    }

    public partial class ClotheSizesGlobalListModel
    {
        /// <summary>
        /// UK Size
        /// </summary>
        public virtual string UKSize { get; set; }
    }

    public partial class ShoeSizesGlobalListModel
    {
        /// <summary>
        /// EU size
        /// </summary>
        public virtual string EUSize { get; set; }
        /// <summary>
        /// UK size
        /// </summary>
        public virtual string UKSize { get; set; }
        /// <summary>
        /// US size
        /// </summary>
        public virtual string USSize { get; set; }
    }

    public partial class PantSizesGlobalListModel
    {
        /// <summary>
        /// Waist
        /// </summary>
        public virtual string Waist { get; set; }
        /// <summary>
        /// Leg
        /// </summary>
        public virtual string Leg { get; set; }
    }

    public partial class ProductHighlightsGlobalListModel
    {
        /// <summary>
        /// Enter the text to use for the highlight
        /// </summary>
        public virtual List<LocalizedData<string>> Text { get; set; }
        /// <summary>
        /// Select the type highlight to use
        /// </summary>
        public virtual ProductHighlightTypeGlobalListModel Type { get; set; }
    }

    public partial class FilterTypesGlobalListModel
    {
        /// <summary>
        /// Enter an alias for this filter type that can be used to recognize how this filter shall be displayed to the end user
        /// </summary>
        public virtual string Alias { get; set; }
    }

    public partial class ProductHighlightTypeGlobalListModel
    {
        /// <summary>
        /// Alias
        /// </summary>
        public virtual string Alias { get; set; }
    }

    public partial class SalesChannelsGlobalListModel
    {
        /// <summary>
        /// Name of sales channel
        /// </summary>
        public virtual string Name { get; set; }
    }


    #endregion
}

