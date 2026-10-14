using CorePay.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Features.Commands.GroupProposal.Post
{
    public class PostGroupProposalCommandHandler : IRequestHandler<PostGroupProposalCommand,Result>
    {
        public PostGroupProposalCommandHandler()
        {
            
        }
        public Task Handle(PostGroupProposalCommand request, CancellationToken cancellationToken)
        {
            
        }
    }
}
