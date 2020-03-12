using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Client1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Clear();
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if(disco.IsError)
            {
                 Console.WriteLine(disco.Error);
                 return;
            }
            Console.WriteLine("Informe cliente1, usuario1 ou usuario2.");
            var users = new string[]{"usuario1","usuario2"}.ToList();
            var info = Console.ReadLine();
            if (info.ToLower().Equals("cliente1"))
            {
                

                var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
                {
                        Address = disco.TokenEndpoint,
                        ClientId = "client",
                        ClientSecret = "secret",
                        Scope = "api1"
                });
                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                    return;
                }
                Console.WriteLine(tokenResponse.Json);

            }
            else if(users.Exists(item => info.ToLower().Equals(item)))
            {
                var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
                {
                        Address = disco.TokenEndpoint,
                        ClientId = "clientpass",
                        ClientSecret = "secret",
                        UserName = info.ToLower(),
                        Password = "123456",
                        Scope = "api1"
                });
                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                    return;
                }
                Console.WriteLine(tokenResponse.Json);

            }


        }

        
    }
}
