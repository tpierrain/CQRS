using System.Collections.Generic;
using BookARoom.Domain.ReadModel;

namespace BookARoom.Infra.Web.ViewModels
{
    public class ReservationsViewModel
    {
        public string ClientEMail { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }

        public ReservationsViewModel()
        {
        }

        public ReservationsViewModel(string clientEMail, IEnumerable<Reservation> reservations)
        {
            ClientEMail = clientEMail;
            Reservations = reservations;
        }
    }
}
