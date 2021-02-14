namespace Rental.API
{
    public struct ApiRoutes
    {
        const string Root = "api";
        const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public struct Identity
        {
            public const string Register = Base + "/identity";
            public const string Login = Base + "/identity/login";
            public const string CreateRole = Base + "/identity/roles";
            public const string CreateUser = Base + "/identity/users";
            public const string GetRoleById = Base + "/identity/roles/{id}";
            public const string GetUserById = Base + "/identity/users/{id}";
            public const string GetRoles = Base + "/identity/roles";
            public const string GetUsers = Base + "/identity/users";
        }

        public struct Auth
        {
            public const string CreateRole = Base + "/auth";
            public const string CreateUser = Base + "/auth/users";
            public const string GetRoleById = Base + "/auth/{id}";
            public const string GetUserById = Base + "/auth/users/{id}";
            public const string GetRoles = Base + "/auth";
            public const string GetUsers = Base + "/auth/users";
        }

        public struct Aircraft
        {
            public const string Create = Base + "/aircraft";
            public const string Get = Base + "/aircraft";
            public const string GetById = Base + "/aircraft/{id}";
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
        }
    }
}
