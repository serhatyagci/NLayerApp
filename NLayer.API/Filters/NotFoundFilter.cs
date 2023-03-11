using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Filters
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity //id alabilmemiz için baseentity geliyor.
    {
        private readonly Iservice<T> _service;

        public NotFoundFilter(Iservice<T> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var idValue = context.ActionArguments.Values.FirstOrDefault(); //propertydeki gelen ilk değeri al yani id al.

            if (idValue == null)
            {
                await next.Invoke(); //eğer id boş ise devam et.
            }

            var id = (int)idValue;
            var anyEntity = await _service.AnyAsync(x => x.Id == id);//id varmı yok mu bilgisi anyentitye atanıyor.

            if (anyEntity)//any entity var ise.
            {
                await next.Invoke();
                return;
            }

            context.Result = new NotFoundObjectResult(CustomResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name}({id}) not found."));

        }
    }
}
