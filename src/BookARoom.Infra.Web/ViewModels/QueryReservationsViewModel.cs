namespace BookARoom.Infra.Web.ViewModels
{
    public class QueryReservationsViewModel
    {
        public string ClientMail { get; set; }

        public QueryReservationsViewModel()
        {
        }

        public QueryReservationsViewModel(string clientMail)
        {
            ClientMail = clientMail;
        }
    }
}
