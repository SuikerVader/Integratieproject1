namespace Integratieproject1.BL.Models.Users
{
    public class Organisation : LoggedInUser
    {
        public string OrganisationName { get; set; }
        public string TaxNumber { get; set; }
    }
}
