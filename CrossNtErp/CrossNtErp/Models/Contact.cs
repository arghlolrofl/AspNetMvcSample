using PropertyChanged;

namespace CrossNtErp.Models {
    [ImplementPropertyChanged]
    public class Contact {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
