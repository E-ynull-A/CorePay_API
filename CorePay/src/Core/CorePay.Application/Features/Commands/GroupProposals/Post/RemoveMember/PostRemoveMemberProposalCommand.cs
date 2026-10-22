using CorePay.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.GroupProposals.Post.RemoveMember
{
    public record PostRemoveMemberProposalCommand(string TargetUserId):IRequest<Result>;
   
}
