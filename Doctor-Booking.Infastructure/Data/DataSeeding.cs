using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Infastructure.Data
{
	public class DataSeeding(DoctorBookingDbContext _dbContext,
							 UserManager<ApplicationUser> _userManager,
							 RoleManager<IdentityRole<int>> _roleManager) : IDataSeeding
	{


		public async Task SeedUsersAsync()
		{
			// Check if users already exist
			if (_userManager.Users.Any())
				return;

            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "john.doe",
                    Email = "john.doe@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    BirthDate = new DateOnly(1990, 1, 1),
                    IsActive = true
                },
                new ApplicationUser
                {
                    UserName = "jane.smith",
                    Email = "jane.smith@example.com",
                    FirstName = "Jane",
                    LastName = "Smith",
                    BirthDate = new DateOnly(1992, 5, 15),
                    IsActive = true
                },
                new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "admin@example.com",
                    FirstName = "admin",
                    LastName = "coonect",
                    BirthDate = new DateOnly(1992, 5, 15),
                    IsActive = true,
                    PhoneNumber ="+201234567891"
                }
               
            };

			foreach (var user in users)
			{
				// You can set a default password for seeding
				await _userManager.CreateAsync(user, "P@ssword123");
			}
		}

		public async Task IdentityDataSeedAsync()
		{
			try
			{
				if (!_roleManager.Roles.Any())
				{
					//Create roles
					await _roleManager.CreateAsync(new IdentityRole<int>("Admin"));
					await _roleManager.CreateAsync(new IdentityRole<int>("SuperAdmin"));
					await _roleManager.CreateAsync(new IdentityRole<int>("Patient"));
					await _roleManager.CreateAsync(new IdentityRole<int>("Doctor"));
				}
				if (!_userManager.Users.Any())
				{
					var user01 = new ApplicationUser()
					{
						Email = "mohamedkilaney2@gmail.com",

						UserName = "MohamedKilaney2",
						PhoneNumber = "01095678452"

					};
					var user02 = new ApplicationUser()
					{
						Email = "Aboseada@gmail.com",

						UserName = "Aboseada2",
						PhoneNumber = "01095678453"
					};

					//Add users to the database with password
					await _userManager.CreateAsync(user01, "P@ssw0rd");
					await _userManager.CreateAsync(user02, "P@ssw0rd");

					//Assign roles to the users
					await _userManager.AddToRoleAsync(user01, "Admin");
					await _userManager.AddToRoleAsync(user02, "SuperAdmin");
				}
			}
			catch (Exception ex)
			{

				//To Do
			}
		}
	}
}