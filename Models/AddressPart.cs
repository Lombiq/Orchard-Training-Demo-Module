using Orchard.ContentManagement;

namespace OrchardHUN.TrainingDemo.Models
{
    public class AddressPart : ContentPart
    {
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
    }


    public enum AddressType
    {
        WorkAddress,
        HomeAddress
    }
}
