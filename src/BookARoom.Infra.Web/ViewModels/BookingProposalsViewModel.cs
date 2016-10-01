using System.Collections.Generic;
using BookARoom.Domain.ReadModel;

namespace BookARoom.Infra.Web.ViewModels
{
    public class BookingProposalsViewModel
    {
        public SearchRoomQueryViewModel SearchCriterias { get; set; }
        public BookingRequestViewModel BookingRequest { get; set; }
        public string Location { get; set; }
        public IEnumerable<BookingProposal> Proposals { get; set; }
        

        public BookingProposalsViewModel()
        {
        }

        public BookingProposalsViewModel(SearchRoomQueryViewModel searchCriterias, string location, IEnumerable<BookingProposal> proposals)
        {
            this.SearchCriterias = searchCriterias;
            this.Location = location;
            this.Proposals = proposals;
        }
    }
}