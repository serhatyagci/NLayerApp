using Microsoft.AspNetCore.Diagnostics;
using NLayer.Core.DTOs;
using NLayer.Service.Exceptions;
using System.Text.Json;

namespace NLayer.API.Middlewares
{
    public static class UseCustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            //useexceptionhandler exception çalıştığında çalışan middlewaredir. 
            app.UseExceptionHandler(config =>
            {
                //kendi modelimiz dönmek için api uygulaması olduğundan json dönecek.
                //run sonlandırıcı bir middlewaredir. rundan sonra request geri döner.
                config.Run(async context =>
                {
                    //contextin tipi belirtiliyor.
                    context.Response.ContentType = "aplication/json";

                    //uygulamada fırlatılan hata alınıyor. hatayı veren interface iexceptionhandf dir.
                    var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    //status kodu clientın hatası ise 400 değilse 500 döner.
                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,
                        NotFoundException=>404,
                        _ => 500
                    };
                    //contextin statüs kodu veriliyor.
                    context.Response.StatusCode = statusCode;

                    //response oluştu.
                    var response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);

                    //response tipini geriye jsona serilazer yapılıyor, dönebilmek için.
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                });

            });

        }
    }
}
