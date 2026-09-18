using CorePay.Application.Common;
using CorePay.Application.Features.Commands.Transactions.MobileApp.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Interfaces.Services
{
    public interface ITransferService
    {
        Task<Result> HighAmountTransferAsync(Guid userId,
                                             string sessionId,
                                             PendingTransferContext context);

        Task<Result> CheckTransferContextAsync(Guid userId,
                                               string sessionId,
                                               PendingTransferContext context);
    }
}
