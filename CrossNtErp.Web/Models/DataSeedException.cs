namespace CrossNtErp.Web.Models {
    public class DataSeedException : System.Exception {
        public DataSeedException(string msg) : base(msg) {

        }

        public DataSeedException(string msg, System.Exception ex) : base(msg, ex) {

        }
    }

    public class RoleSeedException : DataSeedException {
        public string RoleName { get; set; }

        public RoleSeedException(string roleName, string msg) : base(msg) {
            RoleName = roleName;
        }
    }

    public class UserSeedException : DataSeedException {
        public string UserName { get; set; }

        public UserSeedException(string userName, string msg) : base(msg) {
            UserName = userName;
        }
    }
}
