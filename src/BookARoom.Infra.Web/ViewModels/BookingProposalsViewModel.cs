using System.Collections.Generic;
using BookARoom.Domain.ReadModel;

namespace BookARoom.Infra.Web.ViewModels
{
    public class BookingProposalsViewModel
    {
        public string Location { get; set; }
        public IEnumerable<BookingProposal> Proposals { get; set; }
        
        public BookingProposalsViewModel()
        {
        }

        public BookingProposalsViewModel(string location, IEnumerable<BookingProposal> proposals)
        {
            this.Location = location;
            this.Proposals = proposals;
        }
    }
}