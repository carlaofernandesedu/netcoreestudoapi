using System.Collections.Generic;

namespace TweetBook.Domain
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }

        public IEnumerable<string> Errors {get;set;}
        public string Token { get;set;}
    }
}