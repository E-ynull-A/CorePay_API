using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        ICardRepository CardRepository { get; }
        ITransactionRepository TransactionRepository { get; }


        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default);
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
    }
}
