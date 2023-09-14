using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreMVC_WebApi_Test.Migrations
{
    /// <inheritdoc />
    public partial class seedusers : Migration
    {
        // User: alex@ya.ru Pass: Qwerty_123
        // User: admin@ya.ru Pass: Qwerty_123
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT INTO [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName]) VALUES (N'8a04b3f8-cb8e-40b8-a7bc-542c05cf98ee', 0, N'74f42580-c773-4854-90ae-433e8d531bb9', N'admin@ya.ru', 0, 1, NULL, N'ADMIN@YA.RU', N'ADMIN@YA.RU', N'AQAAAAEAACcQAAAAEF9QfFHN4/AlfxIc8XZT4rxYKfArqEwEk6zQwQbTgWAsLjvJI9t/IMaNS+QOfHq8gA==', NULL, 0, N'5f116baf-5d11-46ba-b8ae-e458fdbbc89e', 0, N'admin@ya.ru')
INSERT INTO [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName]) VALUES (N'bf36cad8-18da-42c5-be82-9b58d561bae1', 0, N'ce405c4a-5d34-401b-8e1a-d2faa04b32c7', N'alex@ya.ru', 0, 1, NULL, N'ALEX@YA.RU', N'ALEX@YA.RU', N'AQAAAAEAACcQAAAAEDLAGnnB2lRRvdGbUrdPOa17sfDKmKiPcX9ou2IXt1twMptsj4vXGZ67yUBWoBE81g==', NULL, 0, N'e1f949f0-feea-4a29-a780-64e5f13c382d', 0, N'alex@ya.ru')

INSERT INTO [dbo].[AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName]) VALUES (N'995fefc6-8ca0-4b17-84f7-100bb4fef422', N'c10e39dd-128f-40e8-9fc7-0238385fb564', N'Admin', N'ADMIN')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'8a04b3f8-cb8e-40b8-a7bc-542c05cf98ee', N'995fefc6-8ca0-4b17-84f7-100bb4fef422')

SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (1, N'Position', N'Admin', N'8a04b3f8-cb8e-40b8-a7bc-542c05cf98ee')
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
