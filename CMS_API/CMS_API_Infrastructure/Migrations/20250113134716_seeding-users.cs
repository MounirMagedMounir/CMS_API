using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS_API_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedingusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[]
    {
         "f90c2289-6bbf-4c0b-8c47-24b8beab65fc", "John Doe", "johndoe", "johndoe@example.com", "01123581357", "John@1234", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2024, 2, 15), new DateTime(2024, 2, 15) }
    );
            migrationBuilder.InsertData(
         table: "User",
         columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
         values: new object[] { "07f8a5c0-3f88-460b-b6c0-8be2c2a2798a", "Jane Smith", "janesmith", "janesmith@example.com", "01142038457", "Jane@4321", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2024, 3, 5), new DateTime(2024, 3, 5) }


         );
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "ab6783f2-46a2-4de9-81f5-bce9935a74c1", "Alice Brown", "alicebrown", "alicebrown@example.com", "01234567891", "Alice@8765", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", false, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2024, 4, 10), new DateTime(2024, 4, 10) }
             ); migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "cd3f9b44-b33e-4686-931d-b263453f8ff9", "Michael Green", "michaelgreen", "michaelgreen@example.com", "01459683920", "Michael@5678", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2024, 5, 12), new DateTime(2024, 5, 12) }
); migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "30e49e62-d6a6-41fb-9b25-b1be02942c5a", "Sarah Parker", "sarahparker", "sarahparker@example.com", "01672349827", "Sarah@9876", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2024, 6, 25), new DateTime(2024, 6, 25) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "bb236b8c-d950-47ac-aadd-10bbef86a5c0", "Robert White", "robertwhite", "robertwhite@example.com", "01510928373", "Robert@1923", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", false, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2024, 7, 18), new DateTime(2024, 7, 18) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "ea1b7452-9fcf-4b4a-8003-59770f5c2ef0", "David Harris", "davidharris", "davidharris@example.com", "01987654321", "David@2345", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2024, 8, 5), new DateTime(2024, 8, 5) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "9cf00488-7f99-4e82-9f35-d5b96b85be4b", "Emily Lewis", "emilylewis", "emilylewis@example.com", "01765432109", "Emily@1234", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2024, 9, 10), new DateTime(2024, 9, 10) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "9c909d8e-7097-4669-8e7e-fc573e226b7d", "William Taylor", "williamm", "williamm@example.com", "01987654325", "William@2346", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2024, 10, 12), new DateTime(2024, 10, 12) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "3c90d3d2-bc3f-4534-a5e3-128bbffb7ea7", "Olivia Martin", "oliviam", "oliviam@example.com", "01876543210", "Olivia@3456", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", false, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2024, 11, 25), new DateTime(2024, 11, 25) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "731b537b-bd10-45f0-b503-2b19e5db8974", "Lucas Scott", "lucassc", "lucassc@example.com", "01521048732", "Lucas@4567", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2024, 12, 3), new DateTime(2024, 12, 3) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "7d7d6f44-504e-46f2-baf9-98632c86b2b3", "Ethan Roberts", "ethanroberts", "ethanroberts@example.com", "01452876391", "Ethan@6789", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2025, 1, 15), new DateTime(2025, 1, 15) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "9eb98211-bc85-4a1a-97c0-3f5682cbdc9e", "Ava Wilson", "avawilson", "avawilson@example.com", "01928374615", "Ava@7890", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", false, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2025, 2, 18), new DateTime(2025, 2, 18) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "b519f7e5-90ca-4785-b013-fd20dbfe83f9", "Mia Johnson", "miajohnson", "miajohnson@example.com", "01819374620", "Mia@1234", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2025, 3, 5), new DateTime(2025, 3, 5) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "2d869d3b-84be-4eb8-96a7-f5c62de5db4c", "Amelia Thompson", "ameliathompson", "ameliathompson@example.com", "01795347123", "Amelia@2345", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2025, 4, 7), new DateTime(2025, 4, 7) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "3ae5aebf-3144-4b56-bf69-bd32fcfe8857", "Charlotte Garcia", "charlotteg", "charlotteg@example.com", "01123647859", "Charlotte@3456", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", false, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2025, 5, 15), new DateTime(2025, 5, 15) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "ee52088f-e71b-4649-bb5b-2a48b0cfa76a", "James Miller", "jamesmiller", "jamesmiller@example.com", "01237650981", "James@6789", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "ee52088f-e71b-4649-bb5b-2a48b0cfa76a", new DateTime(2025, 6, 22), new DateTime(2025, 6, 22) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "dbb51273-44d7-4d8d-bcc5-6f1ef156a90c", "Elijah Wright", "elijahw", "elijahw@example.com", "01612345893", "Elijah@7890", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2025, 7, 2), new DateTime(2025, 7, 2) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "ab4c345b-89c3-4126-8b2e-bcb1a6e871eb", "Zoe Adams", "zoeadams", "zoeadams@example.com", "01787654312", "Zoe@6789", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2025, 8, 1), new DateTime(2025, 8, 1) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "e17fefb5-d639-48f2-b433-e4d56a3b8b84", "Jacob Martinez", "jacobm", "jacobm@example.com", "01421237658", "Jacob@1234", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", false, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2025, 8, 15), new DateTime(2025, 8, 15) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "fd3c8457-f798-4bba-a2e5-42c4e8ed3a6b", "Charlotte Lee", "charlottelee", "charlottelee@example.com", "01523847493", "Charlotte@2345", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2025, 9, 18), new DateTime(2025, 9, 18) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "1c9a2eae-e1bc-4f6b-88cf-18f2749bc1f2", "Lily Clark", "lilyclark", "lilyclark@example.com", "01705463527", "Lily@5678", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2025, 10, 11), new DateTime(2025, 10, 11) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "4b4312f2-b0f7-4843-bc89-dba56b340208", "Grace Walker", "gracewalker", "gracewalker@example.com", "01491234567", "Grace@6789", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", false, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2025, 11, 9), new DateTime(2025, 11, 9) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "db5424f7-6f62-41b2-98e2-6c1c6b6a3b12", "David Harris", "davidharris", "davidharris@example.com", "01778345611", "David@2345", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2025, 12, 3), new DateTime(2025, 12, 3) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "68f74cc5-84ac-4381-b9b9-d6762276b888", "Madeline King", "madelineking", "madelineking@example.com", "01609817253", "Madeline@3456", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2026, 1, 25), new DateTime(2026, 1, 25) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "acf9e99b-babf-456d-b49a-9ac2f41365b1", "Samuel Nelson", "samuelnelson", "samuelnelson@example.com", "01526547891", "Samuel@4567", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", false, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2026, 2, 6), new DateTime(2026, 2, 6) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "14bbf8c3-c5b7-4457-9d62-d9da175b7e57", "Sophia Allen", "sophiaallen", "sophiaallen@example.com", "01928974563", "Sophia@5678", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2026, 3, 14), new DateTime(2026, 3, 14) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "372381f2-d6f2-4974-bf95-0ba82b1a50e2", "Evelyn Carter", "evelyncarter", "evelyncarter@example.com", "01782916734", "Evelyn@1234", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2026, 4, 3), new DateTime(2026, 4, 3) });
            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "37e8594c-dacc-4a90-a11e-d9bbfdfb040f", "Henry Young", "henryyoung", "henryyoung@example.com", "01720981567", "Henry@2345", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", false, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2026, 5, 9), new DateTime(2026, 5, 9) });

            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "56007ea2-13ab-4b3b-92e3-d5761f2122f9", "Lillian Harris", "lillianharris", "lillianharris@example.com", "01467298345", "Lillian@5678", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", true, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2026, 6, 3), new DateTime(2026, 6, 3) });

            migrationBuilder.InsertData(
    table: "User",
    columns: new[] { "Id", "Name", "UserName", "Email", "Phone", "Password", "ProfileImage", "IsActive", "RoleId", "CreatedbyId", "LastUpdatedbyId", "CreatedDate", "LastUpdatedDate" },
    values: new object[] { "04029e61-412b-48ff-9e6a-49db7ccf650f", "Asher Moore", "ashermoore", "ashermoore@example.com", "01785632145", "Asher@6789", "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png", false, "6e2b6666-9ef3-4dc0-ad71-bfdb92ab3d8c", "34e53975-bf4e-43b9-baec-629b28a12e14", "34e53975-bf4e-43b9-baec-629b28a12e14", new DateTime(2026, 7, 8), new DateTime(2026, 7, 8) });



        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
