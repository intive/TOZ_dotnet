using System;
using System.Linq;
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

        public Pet Pet { get; }
        public PetsStatus PetsStatus { get; }
        public News News { get; }
        public Organization Organization { get; }
        public User User { get; }
        public Proposal Proposal { get; }
        public HowToHelpInfo HowToHelpInfo { get; set; }
        public JwtToken JwtToken { get; set; }
        public Login Login { get; set; }
        public Helper Helper { get; set; }
        public Comment Comment { get; set; }

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

            PetsStatus = new PetsStatus()
            {
                Id = Guid.NewGuid().ToString(),
                IsPublic = true,
                Name = "testStatusName",
                RGB = "#fc6dff"
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

            this.Proposal = new Proposal()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Mariusz",
                LastName = "Wolonatriusz",
                PhoneNumber = "123456789",
                Email = "test@test.com",
                Roles = new[] { UserType.Volunteer },
                CreationTime = DateTime.Now,
                IsRead = false
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
                },

            };

            HowToHelpInfo = new HowToHelpInfo()
            {
                Description = "Deskrypszyn",
                ModificationTime = DateTime.Now
            };

            JwtToken = new JwtToken()
            {
                UserId = Guid.NewGuid().ToString(),
                Name = "TestName",
                Surname = "TestSurname",
                Roles = new string[] { "TestRole1" },
                Email = "testEmail@test.com",
                ExpirationDateSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 360,
                Jwt = "Token"
            };

            Login = new Login()
            {
                Email = "testEmail@test.com",
                Password = "SecretPassword"
            };

            Helper = new Helper()
            {
                Name = "Mariusz",
                Surname = "Wolonatriusz",
                PhoneNumber = "123456789",
                Email = "test@test.com",
                Address = "Address",
                Category = HelperCategory.Guardian,
                Created = DateTime.Now,
                Id = Guid.NewGuid().ToString(),
                LastModified = DateTime.Now,
                Notes = "notes"
            };

            Comment = new Comment()
            {
                Contents = "Bardzo ładny piesek",
                Id = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                LastModified = DateTime.Now,
                State = CommentState.Active,
                PetUuid = Pet.Id,
                UserUuid = User.Id
            };
        }

        public T DoShallowCopy<T>(T value) where T : new()
        {
            T output = new T();
            var properties = output.GetType().GetProperties();
            foreach (var propertyInfo in properties.Where(prop=> prop.GetSetMethod() != null))
            {
                propertyInfo.SetValue(output, propertyInfo.GetValue(value, null), null);
            }
            return output;
        }
    }
}
