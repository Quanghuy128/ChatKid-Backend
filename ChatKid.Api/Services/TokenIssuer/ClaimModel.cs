﻿namespace ChatKid.Api.Services.TokenIssuer
{
    public class ClaimModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
    }
}
