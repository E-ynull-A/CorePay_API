using CorePay.Application.Interfaces.Repositories;
using CorePay.Domain.Entities;
using CorePay.Persistance.Data_Access_Layer;
using CorePay.Persistance.Implementations.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Persistance.Implementations.Repositories
{
    public class TransactionRepository:Repository<Transaction>,ITransactionRepository
    {
        public TransactionRepository(AppDbContext context):base(context) { }
             
    }
}
