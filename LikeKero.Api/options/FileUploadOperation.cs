
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.Api.options
{
    public class FileUploadOperation : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            if (string.Equals(operation.OperationId, "uploadquestionimage", StringComparison.OrdinalIgnoreCase))
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "uploadedFile",
                    In = ParameterLocation.Path,
                    Description = "Upload File",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "file",
                        Format = "binary"
                    }
                    //Type = "file"
                });

                var uploadFileMediaType = new OpenApiMediaType()
                {
                    Schema = new OpenApiSchema()
                    {
                        Type = "object",
                        Properties =
                    {
                        ["uploadedFile"] = new OpenApiSchema()
                        {
                            Description = "Upload File",
                            Type = "file",
                            Format = "binary"

                        }
                    },
                        Required = new HashSet<string>()
                    {
                        "uploadedFile"
                    }
                    }
                };
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content =
                {
                    ["multipart/form-data"] = uploadFileMediaType
                }
                };

            }
            else if (
                string.Equals(operation.OperationId, "upload", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(operation.OperationId, "getbulkimportuploadedfile", StringComparison.OrdinalIgnoreCase)
              )
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "uploadedFile",
                    In = ParameterLocation.Path,
                    Description = "Upload File",
                    Required = true
                    //Type = "file"
                });
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "uploadedHeaderImage",
                    In = ParameterLocation.Path,
                    Description = "Upload Course Header Image",
                    Required = true
                    //Type = OpenApiParameter.File
                });

                var uploadFileMediaType = new OpenApiMediaType()
                {
                    Schema = new OpenApiSchema()
                    {
                        Type = "object",
                        Properties =
                    {
                        ["uploadedFile"] = new OpenApiSchema()
                        {
                            Description = "Upload File",
                            Type = "file",
                            Format = "binary"

                        }
                    },
                        Required = new HashSet<string>()
                    {
                        "uploadedFile"
                    }
                    }
                };
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content =
                {
                    ["multipart/form-data"] = uploadFileMediaType
                }
                };
            }
        }

        //public void Apply(OpenApiOperation operation, OperationFilterContext context)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
