using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly Iservice<Product> _service;
        private readonly IProductService productService;

        public ProductsController(Iservice<Product> service, IMapper mapper, IProductService productService)
        {
            _service = service;
            _mapper = mapper;
            this.productService = productService;
        }

        // GET ...api/products/GetProductsWithCategory şekkinde çağrılır.
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return createActionResult(await productService.GetProductsWithCategory());
        }


        //GET.../api/product get isteği olduğundan bu kod bloğu çalışacaktır.
        [HttpGet] //httpget isteği olduğunu belirtiyoruz. endpoint.
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync();
            var productsDtos = _mapper.Map<List<ProductDto>>(products.ToList());
            return createActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productsDtos));
        }

        //wwww.mysite.com/api/products/5 şeklinde id 5 olan gelir gibi.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            var productsDto = _mapper.Map<ProductDto>(product);
            return createActionResult(CustomResponseDto<ProductDto>.Success(200, productsDto));
        }


        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
            var productsDto = _mapper.Map<ProductDto>(product);
            return createActionResult(CustomResponseDto<ProductDto>.Success(201, productsDto));
        }


        [HttpPut]  //update
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
            await _service.UpdateASync(_mapper.Map<Product>(productDto));
            return createActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        //DELETE ...api/products/5 dendiğinde 5 silinir.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(product);
            return createActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }


    }
}
