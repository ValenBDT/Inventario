
using AutoMapper;
using Inventory.DTOs;
using Inventory.DTOs.Category;
using Inventory.DTOs.InventoryMovement;
using Inventory.DTOs.InventoryStock;
using Inventory.DTOs.MovementType;
using Inventory.DTOs.Product;
using Inventory.DTOs.Supplier;
using Inventory.Entities;

namespace Inventory.WebAPI.Mapper
{
    public class MappingProfiles: Profile
    {
     
        public MappingProfiles()
        {
            //Category
            CreateMap<CategoryToCreateDTO, Category>();
            CreateMap<CategoryToEditDTO, Category>();
            CreateMap<Category, CategoryToListDTO>();

            //InventoryMovement
            CreateMap<InventoryMovementToCreateDTO, InventoryMovement>();
            CreateMap<InventoryMovementToEditDTO, InventoryMovement>();
            CreateMap<InventoryMovement, InventoryMovementToListDTO>();

            //InventoryStock
            CreateMap<InventoryStockToCreateDTO, InventoryStock>();
            CreateMap<InventoryStockToEditDTO, InventoryStock>();
            CreateMap<InventoryStock, InventoryStockToListDTO>();

            //InventoryTipes

            CreateMap<MovementTypeToCreateDTO, MovementType>();
            CreateMap<MovementTypeToEditDTO, MovementType>();
            CreateMap<MovementType, MovementTypeToListDTO>();

            //Product
            CreateMap<ProductToCreateDTO, Product>();
            CreateMap<ProductToEditDTO, Product>();
            CreateMap<Product, ProductToListDTO>();

            //Supplier
            CreateMap<SupplierToCreateDTO, Supplier>();
            CreateMap<SupplierToEditDTO, Supplier>();
            CreateMap<Supplier, SupplierToListDTO>();


            
        }
    }
}