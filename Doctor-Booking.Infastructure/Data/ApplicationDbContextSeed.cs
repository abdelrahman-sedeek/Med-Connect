using Doctor_Booking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Doctor_Booking.Infastructure.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(DoctorBookingDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();
                

            // Seed roles
            await SeedRolesAsync(roleManager);

            // Seed specialties
            await SeedSpecialtiesAsync(context);

            // Seed users
            await SeedUsersAsync(userManager, context);

            await context.SaveChangesAsync();
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            string[] roles = { "Patient", "Doctor", "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }
        }

        private static async Task SeedSpecialtiesAsync(DoctorBookingDbContext context)
        {
            if (await context.Specialtys.AnyAsync())
                return;

            var specialties = new List<Specialty>
            {
                new Specialty { Name = "Cardiology" },
                new Specialty { Name = "Pediatrics" },
                new Specialty { Name = "Dermatology" },
                new Specialty { Name = "Orthopedics" },
                new Specialty { Name = "Neurology" }
            };

            await context.Specialtys.AddRangeAsync(specialties);
            await context.SaveChangesAsync();
        }

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager, DoctorBookingDbContext context)
        {
            var geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);

            // Check if users already exist
            if (await context.Users.AnyAsync())
                return;

            // 1. Create Patient User
            var patientUser = new ApplicationUser
            {
                UserName = "MohamedAli@test.com",
                Email = "MohamedAli@test.com",
                FirstName = "Mohamed",
                LastName = "Ali",
                PhoneNumber = "01012345678",
                BirthDate = new DateOnly(1995, 5, 15),
                EmailConfirmed = true,
                ProfileImageUrl = "/images/patient-default.png"
            };

            var patientResult = await userManager.CreateAsync(patientUser, "Patient@123");
            if (patientResult.Succeeded)
            {
                await userManager.AddToRoleAsync(patientUser, "Patient");

                // Create Patient entity
                var patient = new Patient
                {
                    UserId = patientUser.Id,
                    Location = geometryFactory.CreatePoint(new Coordinate(31.2357, 30.0444)), // Cairo
                    CreatedAt = DateTime.UtcNow
                };

                await context.Patients.AddAsync(patient);
                await context.SaveChangesAsync();
            }

            // 2. Create Doctor 1 User
            var doctor1User = new ApplicationUser
            {
                UserName = "doctor1@test.com",
                Email = "doctor1@test.com",
                FirstName = "Ahmed",
                LastName = "Hassan",
                PhoneNumber = "01098765432",
                BirthDate = new DateOnly(1980, 3, 20),
                EmailConfirmed = true,
                ProfileImageUrl = "/images/doctor1.jpg"
            };

            var doctor1Result = await userManager.CreateAsync(doctor1User, "Doctor@123");
            if (doctor1Result.Succeeded)
            {
                await userManager.AddToRoleAsync(doctor1User, "Doctor");

                var cardiologySpecialty = await context.Specialtys
                    .FirstOrDefaultAsync(s => s.Name == "Cardiology");

                // Create Doctor 1 entity
                var doctor1 = new Doctor
                {
                    UserId = doctor1User.Id,
                    SpecialtyId = cardiologySpecialty?.Id,
                    About = "Experienced cardiologist with 15 years of practice. Specialized in interventional cardiology and heart disease prevention.",
                    LicenseNumber = "EG-CARD-12345",
                    ClinicName = "Cairo Heart Clinic",
                    Location = geometryFactory.CreatePoint(new Coordinate(31.2400, 30.0500)), // ~0.8 km from patient
                    SessionPrice = 500.00m,
                    IsDoctorApproved = true,
                    IsChangedPassword = true,
                    CreatedAt = DateTime.UtcNow
                };

                await context.Doctors.AddAsync(doctor1);
                await context.SaveChangesAsync();

                // Add availability slots for doctor 1
                var availabilitySlots1 = new List<AvailabilitySlot>
                {
                    new AvailabilitySlot
                    {
                        DoctorId = doctor1.Id,
                        StartTime = DateTime.Today.AddDays(1).AddHours(9),  // Tomorrow 9 AM
                        EndTime = DateTime.Today.AddDays(1).AddHours(10),   // Tomorrow 10 AM
                        IsBooked = false
                    },
                    new AvailabilitySlot
                    {
                        DoctorId = doctor1.Id,
                        StartTime = DateTime.Today.AddDays(1).AddHours(11), // Tomorrow 11 AM
                        EndTime = DateTime.Today.AddDays(1).AddHours(12),   // Tomorrow 12 PM
                        IsBooked = false
                    },
                    new AvailabilitySlot
                    {
                        DoctorId = doctor1.Id,
                        StartTime = DateTime.Today.AddDays(2).AddHours(10), // Day after 10 AM
                        EndTime = DateTime.Today.AddDays(2).AddHours(11),   // Day after 11 AM
                        IsBooked = false
                    }
                };

                await context.AvailabilitySlots.AddRangeAsync(availabilitySlots1);

                //// Add reviews for doctor 1
                //var reviews1 = new List<Review>
                //{
                //    new Review
                //    {
                //        DoctorId = doctor1.Id,
                //        PatientId = patientUser.Id,
                //        Rating = 5m,
                //        Comment = "Excellent doctor, very professional and caring!",
                //        CreatedAt = DateTime.UtcNow.AddDays(-10)
                //    },
                //    //new Review
                //    //{
                //    //    DoctorId = doctor1.Id,
                //    //    PatientId = patientUser.Id,
                //    //    Rating = 4.5m,
                //    //    Comment = "Great experience, highly recommended.",
                //    //    CreatedAt = DateTime.UtcNow.AddDays(-5)
                //    //}
                //};

                //await context.Reviews.AddRangeAsync(reviews1);
                //await context.SaveChangesAsync();
            }

            // 3. Create Doctor 2 User
            var doctor2User = new ApplicationUser
            {
                UserName = "doctor2@test.com",
                Email = "doctor2@test.com",
                FirstName = "Sara",
                LastName = "Mohamed",
                PhoneNumber = "01087654321",
                BirthDate = new DateOnly(1985, 7, 10),
                EmailConfirmed = true,
                ProfileImageUrl = "/images/doctor2.jpg"
            };

            var doctor2Result = await userManager.CreateAsync(doctor2User, "Doctor@123");
            if (doctor2Result.Succeeded)
            {
                await userManager.AddToRoleAsync(doctor2User, "Doctor");

                var pediatricsSpecialty = await context.Specialtys
                    .FirstOrDefaultAsync(s => s.Name == "Pediatrics");

                // Create Doctor 2 entity
                var doctor2 = new Doctor
                {
                    UserId = doctor2User.Id,
                    SpecialtyId = pediatricsSpecialty?.Id,
                    About = "Pediatrician with a passion for children's health. 10 years of experience in child care and development.",
                    LicenseNumber = "EG-PED-67890",
                    ClinicName = "Kids Care Center",
                    Location = geometryFactory.CreatePoint(new Coordinate(31.2500, 30.0550)), // ~1.5 km from patient
                    SessionPrice = 400.00m,
                    IsDoctorApproved = true,
                    IsChangedPassword = true,
                    CreatedAt = DateTime.UtcNow
                };

                await context.Doctors.AddAsync(doctor2);
                await context.SaveChangesAsync();

                // Add availability slots for doctor 2
                var availabilitySlots2 = new List<AvailabilitySlot>
                {
                    new AvailabilitySlot
                    {
                        DoctorId = doctor2.Id,
                        StartTime = DateTime.Today.AddDays(1).AddHours(10), // Tomorrow 10 AM
                        EndTime = DateTime.Today.AddDays(1).AddHours(11),   // Tomorrow 11 AM
                        IsBooked = false
                    },
                    new AvailabilitySlot
                    {
                        DoctorId = doctor2.Id,
                        StartTime = DateTime.Today.AddDays(1).AddHours(14), // Tomorrow 2 PM
                        EndTime = DateTime.Today.AddDays(1).AddHours(15),   // Tomorrow 3 PM
                        IsBooked = false
                    },
                    new AvailabilitySlot
                    {
                        DoctorId = doctor2.Id,
                        StartTime = DateTime.Today.AddDays(3).AddHours(9),  // 3 days later 9 AM
                        EndTime = DateTime.Today.AddDays(3).AddHours(10),   // 3 days later 10 AM
                        IsBooked = false
                    }
                };

                await context.AvailabilitySlots.AddRangeAsync(availabilitySlots2);

                //// Add reviews for doctor 2
                //var reviews2 = new List<Review>
                //{
                //    new Review
                //    {
                //        DoctorId = doctor2.Id,
                //        PatientId = patientUser.Id,
                //        Rating = 5m,
                //        Comment = "Amazing with kids, my son loved her!",
                //        CreatedAt = DateTime.UtcNow.AddDays(-7)
                //    },
                //    //new Review
                //    //{
                //    //    DoctorId = doctor2.Id,
                //    //    PatientId = patientUser.Id,
                //    //    Rating = 4.8m,
                //    //    Comment = "Very knowledgeable and patient.",
                //    //    CreatedAt = DateTime.UtcNow.AddDays(-3)
                //    //}
                //};

                //await context.Reviews.AddRangeAsync(reviews2);
            }

            await context.SaveChangesAsync();
        }
    }
}
