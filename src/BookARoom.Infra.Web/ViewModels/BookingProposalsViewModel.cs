using System.Collections.Generic;
using BookARoom.Domain.ReadModel;

namespace BookARoom.Infra.Web.ViewModels
{
    public class BookingProposalsViewModel
    {
        public IEnumerable<BookingProposal> Proposals { get; /* TODO: comment the setter */ set; }
        public string Portnaouaq { get; /* TODO: comment the setter */ set; }

        public BookingProposalsViewModel()
        {
        }

        public BookingProposalsViewModel(IEnumerable<BookingProposal> proposals)
        {
            this.Proposals = proposals;
        }
    }
}