using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using WebDevelopment_BCU.Models;

namespace WebDevelopment_BCU
{
    public static class DataSeeder
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var userId = UserConfiguration.MainAdminId;
            var normalUserId = UserConfiguration.NormalUserId;


            var adminUser = new User
            {
                Id = userId,
                FullName = "Admin",
                UserName = "MainAdmin@email.com",
                NormalizedUserName = "MainAdmin@email.com",
                Email = "MainAdmin@email.com",
                NormalizedEmail = "MainAdmin@email.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var normalUser = new User
            {
                Id = normalUserId,
                UserName = "normaluser@email.com",
                NormalizedUserName = "normaluser@email.com",
                Email = "normaluser@email.com",
                NormalizedEmail = "normaluser@email.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var ph = new PasswordHasher<User>();

            adminUser.PasswordHash = ph.HashPassword(adminUser, "123456");
            normalUser.PasswordHash = ph.HashPassword(normalUser, "123456");

            modelBuilder.Entity<User>().HasData(adminUser);
            modelBuilder.Entity<User>().HasData(normalUser);


            var adminRoleId = UserConfiguration.AdminRoleId;
            var normalRoleId = UserConfiguration.NormalUserRoleId;


            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "admin", NormalizedName = "admin", Id = adminRoleId, ConcurrencyStamp = adminRoleId },
                new IdentityRole { Name = "normaluser", NormalizedName = "normaluser", Id = normalRoleId, ConcurrencyStamp = normalRoleId }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = userId.ToString(),

            },
                new IdentityUserRole<string>
                {
                    RoleId = normalRoleId,
                    UserId = normalUserId.ToString()
                }
            );

        }
    }

    public class UserConfiguration
    {
        public const string AdminRoleName = "MainAdmin";
        public const string AdminRoleId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";
        public const string MainAdminId = "02174cf0–9412–4cfe-afbf-59f706d72cf6";

        public const string NormalUserName = "NormalUser";
        public const string NormalUserRoleId = "51cc04c4-e673-478a-9616-f9afbb0fe6f7";
        public const string NormalUserId = "c49c10c7-f5cb-4504-a528-c78a9012a457";

    }
}
