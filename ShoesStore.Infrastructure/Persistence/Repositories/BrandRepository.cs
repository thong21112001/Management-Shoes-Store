using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShoesStore.Application.Common.Interfaces;
using ShoesStore.Application.DTOs;
using ShoesStore.Domain.Entities.Data;
using ShoesStore.Infrastructure.Persistence.Data;

namespace ShoesStore.Infrastructure.Persistence.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BrandRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<BrandsDto>> GetAllBrandsAsync()
        {
            return await _context.Brands
                                .AsNoTracking()
                                .ProjectTo<BrandsDto>(_mapper.ConfigurationProvider)
                                .ToListAsync();
        }

        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            return await _context.Brands.AsNoTracking()
                                 .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Brand> AddAsync(Brand brand)
        {
            brand.CreatedAt = DateTime.UtcNow;
            brand.UpdatedAt = null;
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task UpdateAsync(Brand brand)
        {
            brand.UpdatedAt = DateTime.UtcNow;
            _context.Entry(brand).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }
        }
    }
}
