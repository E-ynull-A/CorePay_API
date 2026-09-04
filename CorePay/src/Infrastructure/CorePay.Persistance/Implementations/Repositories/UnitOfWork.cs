using CorePay.Application.Interfaces.Repositories;
using CorePay.Persistance.Data_Access_Layer;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Persistance.Implementations.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        public UnitOfWork(ITransactionRepository transactionRepository,
                          IAccountRepository accountRepository,
                          ICardRepository cardRepository,
                          ITransferRepository transferRepository,
                          AppDbContext appDbContext)
        {
            TransactionRepository = transactionRepository;
            AccountRepository = accountRepository;
            CardRepository = cardRepository;
            TransferRepository = transferRepository;
            _appDbContext = appDbContext;
        }

        public ITransactionRepository TransactionRepository { get; }
        public IAccountRepository AccountRepository { get; }
        public ICardRepository CardRepository { get; }
        public ITransferRepository TransferRepository { get; }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default)
        {
          return await _appDbContext.Database.BeginTransactionAsync(token);
        }

        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
