﻿using System.Diagnostics.Tracing;

namespace SimpleIdentityServer.Logging
{
    public static class Constants
    {
        public static class EventIds
        {
            public const int AuthorizationStarted = 1;
            public const int AuthorizationCodeFlowStarted = 2;
            public const int StartProcessingAuthorizationRequest = 3;
            public const int EndProcessingAuthorizationRequest = 4;
            public const int StartGeneratingAuthorizationResponseToClient = 5;
            public const int GrantAccessToClient = 6;
            public const int GrantAuthorizationCodeToClient = 7;
            public const int EndGeneratingAuthorizationResponseToClient = 8;
            public const int AuthorizationCodeFlowEnded = 9;
            public const int AuthorizationEnded = 10;
            public const int OpenIdFailure = 11;
        }

        public static class Tasks
        {
            public const EventTask Authorization = (EventTask) 1;
            public const EventTask Failure = (EventTask) 2;
        }
    }
}
