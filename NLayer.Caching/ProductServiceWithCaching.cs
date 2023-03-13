using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Caching
{
    public class ProductServiceWithCaching : IProductService
    {
        private const string CacheProductKey = "productsCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _mermoryCache;
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductServiceWithCaching(IUnitOfWork unitOfWork, IProductRepository repository, IMemoryCache mermoryCache, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mermoryCache = mermoryCache;
            _mapper = mapper;

            if(!_mermoryCache.TryGetValue(CacheProductKey, out _)) //trygetvalue geriyo true dönerse out ile geriye tuttuğu veriyi döner ancak biz sadece _ koyuyoruz true false olduğunu öğreniyoruz.
            {
                _mermoryCache.Set(CacheProductKey, _repository.GetAll().ToList());//eğer cache datasında yok ise memorycahce set ediyoruz, cachliyoruz.
            }


        }

        public Task<Product> AddAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CustomResponseDto<List<ProductWithCaregoryDto>>> GetProductsWithCategory()
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateASync(Product entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> where(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
