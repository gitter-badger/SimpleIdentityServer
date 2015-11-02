﻿namespace SimpleIdentityServer.Core.Errors
{
    public static class ErrorDescriptions
    {
        public static string MissingParameter = "the parameter {0} is missing";

        public static string RequestIsNotValid =  "the request is not valid";

        public static string ClientIsNotValid = "the client_id {0} is not valid";

        public static string RedirectUrlIsNotValid = "the redirect url {0} is not valid";

        public static string ResourceOwnerCredentialsAreNotValid = "resource owner credentials are not valid";

        public static string ParameterIsNotCorrect = "the paramater {0} is not correct";

        public static string ScopesAreNotAllowedOrInvalid = "the scopes {0} are not allowed or invalid";

        public static string DuplicateScopeValues = "the scopes {0} are duplicated";

        public static string TheScopesNeedToBeSpecified = "the scopes {0} need to be specified";

        public static string TheUserNeedsToBeAuthenticated = "the user needs to be authenticated";

        public static string TheUserNeedsToGiveIsConsent = "the user needs to give his consent";

        public static string TheUserCannotBeReauthenticated = "The user cannot be reauthenticated";

        public static string TheRedirectionUriIsNotWellFormed = "Based on the RFC-3986 the redirection-uri is not well formed";
    }
}
