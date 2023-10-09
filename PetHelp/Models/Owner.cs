﻿namespace PetHelp.Models
{
    public class Owner
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public List<Ad>? Ads { get; set; }
        public List<Pet>? Pets { get; set; }
    }
}
