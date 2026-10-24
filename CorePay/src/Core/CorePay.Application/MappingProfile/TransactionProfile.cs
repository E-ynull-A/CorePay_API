using AutoMapper;
using CorePay.Application.Features.Queries.Accounts.GetById;
using CorePay.Application.Features.Queries.Transactions.GetAll;
using CorePay.Application.Features.Queries.Transactions.GetById;
using CorePay.Domain.Entities;
using CorePay.Domain.Utilities.Enums;

namespace CorePay.Application.MappingProfile
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, GetAllTransactionQueryResponse>()
                .ForCtorParam(nameof(GetAllTransactionQueryResponse.Currency)
                                            ,otp => otp.MapFrom((src, context) =>
                                                context.Items.TryGetValue(nameof(Account.Currency), out var currency)
                                                        ? currency
                                                        : new InvalidOperationException("Currency of Account doesn't exist!")
                                            ));

            CreateMap<Transaction, GetByIdTransactionQueryResponse>()
                .ForCtorParam(nameof(GetByIdTransactionQueryResponse.Currency), otp => otp.MapFrom((src, context) =>
                        context.Items.TryGetValue(nameof(Account.Currency), out var currency)
                                            ? currency
                                            : throw new InvalidOperationException("Currency of Account doesn't exist!")
                ))
                .ForCtorParam(nameof(GetByIdTransactionQueryResponse.Transfer), opt => opt.MapFrom((src, context) =>
                                        {
                                            if (src.Type != TransactionType.Transfer)
                                                return null;

                                            return context.Items.TryGetValue(nameof(GetByIdTransactionQueryResponse.Transfer), out var transfer) 
                                                                                        && transfer is not null
                                                ? transfer
                                                : throw new InvalidOperationException($"Transfer details are missing for Transaction '{src.Id}'.");
                                        }));
        }
    }
}
