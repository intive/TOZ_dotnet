using System;
using System.Reflection;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Models.Organization;

namespace Toz.Dotnet.Tests.Helpers
{
    public class TestingObjectProvider
    {
        private static TestingObjectProvider _instance;
        public static TestingObjectProvider Instance => _instance ?? (_instance = new TestingObjectProvider());

        private TestingObjectProvider()
        {
            Pet = new Pet
            {
                Id = Guid.NewGuid().ToString(),
                Name = "TestDog",
                Type = PetType.Dog,
                Sex = PetSex.Male,
                Photo = new byte[10],
                Description = "Dog that eats tigers",
                Address = "Found in the jungle",
                Created = DateTime.Now,
                LastModified = DateTime.Now
            };

            User = new User
            {
                FirstName = "Mariusz",
                LastName = "Wolonatriusz",
                Password = "TajneHasloMariusza",
                PhoneNumber = "123456789",
                Email = "test@test.com",
                Roles = new[] { UserType.Volunteer }
            };

            News = new News
            {
                Id = Guid.NewGuid().ToString(),
                Title = "TestNews",
                Published = DateTime.Now,
                Created = DateTime.Now,
                LastModified = DateTime.Now,
                Contents = "Text",
                Photo = new byte[10],
                Type = NewsStatus.Released
            };

            Organization = new Organization
            {
                Name = "Test",
                Address = new Address
                {
                    ApartmentNumber = "45",
                    City = "TestCity",
                    Country = "TestCountry",
                    HouseNumber = "11",
                    PostCode = "73-220",
                    Street = "TestStreet"
                },

                BankAccount = new BankAccount
                {
                    BankName = "TestBankName",
                    Number = "61109010140000071219812874"
                },

                Contact = new Contact
                {
                    Email = "testEmail@test.com",
                    Fax = "123456789",
                    Phone = "123456789",
                    Website = "http://testwebsite.com"
                }
            };
        }

        public Pet Pet { get; }
        public News News { get; }
        public Organization Organization { get; }
        public User User { get; }

        public T DoShallowCopy<T>(T value) where T : new()
        {
            T output = new T();
            var properties = output.GetType().GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                pi.SetValue(output, pi.GetValue(value, null), null);
            }
            return output;
        }

    }
}
