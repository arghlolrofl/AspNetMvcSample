using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrossNtErp.Shared.Entities.Base {
    public abstract class EntityBase {
        private long _id;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id {
            get { return _id; }
            set { _id = value; }
        }

        [NotMapped]
        public bool Exists {
            get { return Id > 0; }
        }
    }
}