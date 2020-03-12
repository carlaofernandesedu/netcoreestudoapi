// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4.Test;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[] { new ApiResource("api1", "My API") };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "usuario1",
                    Password = "123456"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "usuario2",
                    Password = "123456"
                }
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new Client[] { 
                new Client{
                    ClientId="client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())

                    },
                    AllowedScopes = {"api1"}
                },
                new Client{
                    ClientId="clientpass",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())

                    },
                    AllowedScopes = {"api1"}
                },
            };
        }
    }
}