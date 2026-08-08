using CorePay.Domain.Utilities.Enums;
using CorePay.Domain.Utilities.Errors.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public Error Error { get; set; }
        public Result(bool isSuccess,Error error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public Result Success() =>
            new Result(true,null);

        public Result Failure(Error error) =>
            new Result(false, error);
    }

    public class Result<T>:Result
    {
        public T Value { get; set; }

        public Result(T value):base(true,null)
        {
            Value = value;
        }

        public Result(Error error):base(false,error)
        {}

        public Result<T> Success(T value)=>
            new Result<T>(value);

        public Result<T> Failure(Error error) =>
            new Result<T>(error);
    }
    
}
