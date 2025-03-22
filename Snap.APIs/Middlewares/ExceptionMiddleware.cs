using Snap.APIs.Errors;
using System.Net;
using System.Text.Json;

namespace Snap.APIs.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleware(RequestDelegate Next , ILogger<ExceptionMiddleware>logger , IHostEnvironment environment)
        {
            _next = Next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext Context) 
        {

            try 
            {
            
           await _next.Invoke(Context);
            
            
            
            
            
            } 
            catch (Exception ex) 
            {

                _logger.LogError(ex, ex.Message);
            Context.Response.ContentType= "application/json"; 
            Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
               // if (_environment.IsDevelopment())
               // {
               //     var response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError ,ex.Message, ex.StackTrace.ToString());




               // }
               // else {

               //var response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);



                //}

   var Response = _environment.IsDevelopment() ?
  new ApiExceptionResponse((int)HttpStatusCode.InternalServerError ,ex.Message, ex.StackTrace.ToString()):
  new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                var Options = new JsonSerializerOptions()
                {

                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase

                };
                var JsonResponse = JsonSerializer.Serialize(Response , Options);
              await  Context.Response.WriteAsync(JsonResponse);
            }


        }





    }
}
