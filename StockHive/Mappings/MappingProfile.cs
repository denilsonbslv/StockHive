// Mappings/MappingProfile.cs
using AutoMapper;
using StockHive.DTOs.Category;
using StockHive.DTOs.Supplier;
using StockHive.Models;

namespace StockHive.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeamento para Supplier
        CreateMap<Supplier, SupplierDto>(); // De Entidade para DTO
        CreateMap<CreateSupplierDto, Supplier>(); // De DTO para Entidade
        CreateMap<UpdateSupplierDto, Supplier>(); // De DTO para Entidade

        CreateMap<Category, CategoryDto>(); // De Entidade para DTO
        CreateMap<CreateCategoryDto, Category>(); // De DTO para Entidade
        CreateMap<UpdateCategoryDto, Category>(); // De DTO para Entidade
    }
}