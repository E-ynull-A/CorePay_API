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
        public bool IsSuccess { get;}
        public Error Error { get; }
        public Result(bool isSuccess,Error error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() =>
            new (true,null);

        public static Result Failure(Error error) =>
            new (false, error);
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

        public static Result<T> Success(T value)=>
            new Result<T>(value);

        public static Result<T> Failure(Error error) =>
            new Result<T>(error);
    }
    
}
