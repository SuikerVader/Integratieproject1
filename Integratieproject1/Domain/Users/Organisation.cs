namespace Integratieproject1.Domain.Users
{
    public class Organisation : LoggedInUser
    {
        public string OrganisationName { get; set; }
        public string TaxNumber { get; set; }
    }
}
