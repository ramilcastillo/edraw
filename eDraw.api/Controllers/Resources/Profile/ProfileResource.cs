namespace eDraw.api.Controllers.Resources.Profile
{
    public class BankProfileResource
    {
        public string BankName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class HomeOwnerProfileResource
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class SubContractorProfileResource
    {
        public string BussinesName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string TaxId { get; set; }
        public string W9 { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public string OriginatingPartyName { get; set; }
        public string ReceivingPartyName { get; set; }
    }

}
