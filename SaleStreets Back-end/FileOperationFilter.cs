using System.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;

public class FileOperationFilter:IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParams = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(IEnumerable<IFormFile>));

        foreach(var param in fileParams)
        {
            var uploadFileMediaType = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {

                    Type = "array",
                    Items = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary",
                        Example = new OpenApiString("base64-encoded-image-data")
                    },
                   // 
                }
            };

        
        }
    }
}