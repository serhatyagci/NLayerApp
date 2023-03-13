using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;
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
                _mermoryCache.Set(CacheProductKey, _repository.GetProductsWithCategory());//eğer cache datasında yok ise memorycahce set ediyoruz, cachliyoruz. hem category hemde productları cachliyoruz.
            }


        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts(); //aşağıdaki cacheleme metodu çağrılıyor.
            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(_mermoryCache.Get<IEnumerable<Product>>(CacheProductKey));
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var product = _mermoryCache.Get<List<Product>>(CacheProductKey).FirstOrDefault(x => x.Id == id); //cachede id var ise cahcden döner.
            if (product == null)
            {
                throw new NotFoundException($"{typeof(Product).Name}({id}) not fount."); //istenilen product id yopk ise exception fırlatır.
            }
            return Task.FromResult(product);
        }

        public Task<CustomResponseDto<List<ProductWithCaregoryDto>>> GetProductsWithCategory()
        {
            var products = _mermoryCache.Get<IEnumerable<Product>>(CacheProductKey);
            var productsWithCategoryDto = _mapper.Map<List<ProductWithCaregoryDto>>(products);
            return Task.FromResult( CustomResponseDto<List<ProductWithCaregoryDto>>.Success(200, productsWithCategoryDto));
        }

        public async Task RemoveAsync(Product entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
        }

        public async Task UpdateASync(Product entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
        }

        public IQueryable<Product> where(Expression<Func<Product, bool>> expression)
        {
            return _mermoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
        }

        public async Task CacheAllProducts()
        {
            // her çağrıldığında sıfırdan datayı çekip cechliyor.
            _mermoryCache.Set(CacheProductKey, await _repository.GetAll().ToListAsync());
        }
    }
}
