﻿namespace SimpleIdentityServer.DataAccess.SqlServer.Models
{
    public class Scope
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsInternal { get; set; }
    }
}
