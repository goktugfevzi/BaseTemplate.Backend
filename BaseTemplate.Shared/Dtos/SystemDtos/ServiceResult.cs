using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseTemplate.Shared.Dtos.SystemDtos
{
    public class ServiceResult<T>
    {
        public T Data { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; }


        public List<string> Errors { get; set; }

        public int TotalItemCount { get; set; }

        public static ServiceResult<T> Success(int statusCode, T data, int totalItemCount)
        {
            return new ServiceResult<T> { Data = data, StatusCode = statusCode, TotalItemCount = totalItemCount };
        }
        public static ServiceResult<T> Success(int statusCode, T data)
        {
            return new ServiceResult<T> { Data = data, StatusCode = statusCode };
        }

        public static ServiceResult<T> Fail(int statusCode, List<string> errors)
        {
            return new ServiceResult<T> { StatusCode = statusCode, Errors = errors };
        }

        public static ServiceResult<T> Fail(int statusCode, string error)
        {
            return new ServiceResult<T> { StatusCode = statusCode, Errors = new List<string> { error } };
        }
    }
}
