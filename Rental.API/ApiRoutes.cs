namespace Rental.API
{
    public struct ApiRoutes
    {
        const string Root = "api";

        public struct V1
        {
            const string Version = "v1";
            public const string Base = Root + "/" + Version;

            public struct Identity
            {
                public const string Register = Base + "/identity";
                public const string Login = Base + "/identity/login";
                public const string UserExists = Base + "/identity/users/exists";
                public const string CreateRole = Base + "/identity/roles";
                public const string CreateUser = Base + "/identity/users";
                public const string GetRoleById = Base + "/identity/roles/{id}";
                public const string GetUserById = Base + "/identity/users/{id}";
                public const string GetRoles = Base + "/identity/roles";
                public const string GetUsers = Base + "/identity/users";
                public const string GetPermissionsByRole = Base + "/identity/roles/{idRole}/permissions";
            }

            public struct Aircraft
            {
                public const string Create = Base + "/aircraft";
                public const string Get = Base + "/aircraft";
                public const string GetById = Base + "/aircraft/{id}";
                public const string GetByState = Base + "/aircraft/state/{state}";
                public const string Update = Base + "/aircraft/{id}";
                public const string Delete = Base + "/aircraft/{id}";
            }

            public struct Rental
            {
                public const string CreateRental = Base + "/rental";
                public const string CreatePassenger = Base + "/rental/passengers";
                public const string GetRentalById = Base + "/rental/{id}";
                public const string GetPassengerById = Base + "/rental/passengers/{id}";
                public const string GetRentals = Base + "/rental";
                public const string GetPassengers = Base + "/rental/passengers";
                public const string GetPassengersNotAvailable = Base + "/rental/passengers/notavailable";
            }
        }
    }
}
