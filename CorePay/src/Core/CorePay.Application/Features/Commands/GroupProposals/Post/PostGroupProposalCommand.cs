using CorePay.Application.Common;
using CorePay.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.GroupProposals.Post
{
    public record PostGroupProposalCommand(Guid GroupMemberId,
                                           ProposalType Type,
                                           decimal? Amount):IRequest<Result>;
  
}
