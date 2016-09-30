using System.Collections.Generic;
using BookARoom.Domain.ReadModel;

namespace BookARoom.Infra.Web.ViewModels
{
    public class BookingProposalsViewModel
    {
        private IEnumerable<BookingProposal> proposals;

        public IEnumerable<BookingProposal> Proposals
        {
            get
            {
                if (this.proposals == null)
                {
                    this.proposals = new List<BookingProposal>(); // TODO/ remove this hack
                }

                return this.proposals;
            }
            set
            {
                this.proposals = value;
            }
        }

        public string Location { get; /* TODO: comment the setter */ set; }

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