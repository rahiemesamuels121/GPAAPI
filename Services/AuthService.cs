using GPACARICOMAPI.Models;
using GPACARICOMAPI.Models.DTO;
using GPACARICOMAPI.Services.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Primitives;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;


namespace GPACARICOMAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;
        private readonly IVerificationService _verificationService;
        private readonly IEmailService _emailService;


        public AuthService(IConnectionFactory dbconn, IConfiguration configuration, IVerificationService verificationService, IEmailService emailService)
        {
            _connectionFactory = dbconn;
            _configuration = configuration;
            _verificationService = verificationService;
            _emailService = emailService;
        }

        //public async Task<UserLoginResponse> Login(UserLoginRequest request)
        //{
        //    int userId;
        //    string firstname;
        //    string lastname;
        //    string email;
        //    string role;
        //    using var connection = _connectionFactory.GetConnection();

        //    await connection.OpenAsync();

        //    string sql =
        //    @"
        //    SELECT 
        //        u.id,
        //        u.firstname,
        //        u.lastname,
        //        u.email,
        //        a.password_hash,
        //        a.password_salt,
        //        r.role_id,
        //        r.role_name
        //    FROM users u

        //    INNER JOIN auth a
        //        ON u.id = a.user_id

        //    INNER JOIN roles r
        //        ON u.role_id = r.role_id

        //    WHERE u.email = @email
        //    AND u.is_verified = 1;
        //    ";

        //    using var cmd = new MySqlCommand(sql, connection);
        //    cmd.Parameters.AddWithValue("@email", request.Username);
        //    using var reader = await cmd.ExecuteReaderAsync();
        //    if (!await reader.ReadAsync())
        //        return null;
        //    byte[] storedSalt = reader.GetFieldValue<byte[]>(reader.GetOrdinal("password_salt"));
        //    byte[] storedHash = reader.GetFieldValue<byte[]>(reader.GetOrdinal("password_hash"));
        //    userId = reader.GetInt32("id");
        //    firstname = reader.GetString("firstname");
        //    lastname = reader.GetString("lastname");
        //    email = reader.GetString("email");
        //    role = reader.GetString("role_name");
        //    byte[] passwordhash = GeneratePasswordhash(request.Password, storedSalt);

        //    for (int index = 0; index < passwordhash.Length; index++) {
        //        if (passwordhash[index] != storedHash[index]) {
        //            return null;
        //        }
        //    }

        //    string updateLoginSql =
        //    @"
        //        UPDATE users
        //        SET last_login_at = UTC_TIMESTAMP()
        //        WHERE id = @userId;
        //    ";

        //    using var updateCmd =
        //        new MySqlCommand(
        //            updateLoginSql,
        //            connection);

        //    updateCmd.Parameters.AddWithValue(
        //        "@userId",
        //        userId);
        //    await updateCmd.ExecuteNonQueryAsync();

        //    return new UserLoginResponse(){ 
        //      UserId = userId,
        //      FirstName = firstname,
        //      LastName = lastname,
        //      Email = email,
        //      Role = role,
        //    };
        //}


        public async Task<UserLoginResponse?> Login(
    UserLoginRequest request)
        {
            int userId;
            string firstname;
            string lastname;
            string email;
            string role;

            await using var connection =
                _connectionFactory.GetConnection();

            await connection.OpenAsync();

            const string sql = """
        SELECT
            u.id,
            u.firstname,
            u.lastname,
            u.email,
            a.password_hash,
            a.password_salt,
            r.role_id,
            r.role_name
        FROM users u

        INNER JOIN auth a
            ON u.id = a.user_id

        INNER JOIN roles r
            ON u.role_id = r.role_id

        WHERE u.email = @email
          AND u.is_verified = 1;
        """;

            byte[] storedSalt;
            byte[] storedHash;

            await using (var cmd =
                new MySqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue(
                    "@email",
                    request.Username);

                await using var reader =
                    await cmd.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                {
                    return null;
                }

                storedSalt =
                    reader.GetFieldValue<byte[]>(
                        reader.GetOrdinal("password_salt"));

                storedHash =
                    reader.GetFieldValue<byte[]>(
                        reader.GetOrdinal("password_hash"));

                userId =
                    reader.GetInt32("id");

                firstname =
                    reader.GetString("firstname");

                lastname =
                    reader.GetString("lastname");

                email =
                    reader.GetString("email");

                role =
                    reader.GetString("role_name");
            }

            // Reader is now CLOSED/DISPOSED.

            var passwordHash =
                GeneratePasswordhash(
                    request.Password,
                    storedSalt);

            if (!CryptographicOperations.FixedTimeEquals(
                    passwordHash,
                    storedHash))
            {
                return null;
            }

            const string updateLoginSql = """
        UPDATE users
        SET last_login_at = UTC_TIMESTAMP()
        WHERE id = @userId;
        """;

            await using (var updateCmd =
                new MySqlCommand(
                    updateLoginSql,
                    connection))
            {
                updateCmd.Parameters.AddWithValue(
                    "@userId",
                    userId);

                await updateCmd.ExecuteNonQueryAsync();
            }

            return new UserLoginResponse
            {
                UserId = userId,
                FirstName = firstname,
                LastName = lastname,
                Email = email,
                Role = role
            };
        }

        public async Task<SignUpResponse> Signup(AppUserDTO user)
        
        {

            using var connection =  _connectionFactory.GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            int _userId;
            try {
                string addToUsers =
             @"INSERT INTO users
             (
              role_id,
              firstname,
              lastname,
              email,
              organization_name,
              telephone,
              created_by,
              created_date,
              last_login_at,
              is_deleted,
              is_verified
            )
            Values
                (
                    @roleId,
                    @firstname,
                    @lastname,
                    @email,
                    @organizationName,
                    @telephone,
                    @createdBy,
                    @createdDate,
                    @lastLoginAt,
                    @isDeleted,
                    @isVerified
                );

                SELECT LAST_INSERT_ID();
                 ";
                using var command = new MySqlCommand(addToUsers, connection, transaction);
                command.Parameters.AddWithValue("@roleId", user.RoleId.ToString());
                command.Parameters.AddWithValue("@firstname", user.Firstname);
                command.Parameters.AddWithValue("@lastname", user.Lastname);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@organizationName", user.Organization);
                command.Parameters.AddWithValue("@telephone", user.Telephone);
                command.Parameters.AddWithValue("@createdBy", "Admin");
                command.Parameters.AddWithValue("@lastLoginAt", DateTime.Now);
                command.Parameters.AddWithValue("@createdDate", DateTime.Now);
                command.Parameters.AddWithValue("@isDeleted", false);
                command.Parameters.AddWithValue("@isVerified", false);
                _userId = Convert.ToInt32(await command.ExecuteScalarAsync());


                byte[] passwordSalt = new byte[128 / 8];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetNonZeroBytes(passwordSalt);
                }
                byte [] passwordHash = GeneratePasswordhash(user.Password, passwordSalt);

                    string addToauth = @"INSERT INTO auth
            (
              password_hash,
              last_login_at,
              role_id,
              created_date,
              created_by,
              user_id,
              password_salt
            )
            Values
            (
              @passwordHash,
              @lastLoginAt,
              @roleId,
              @createdDate,
              @createdBy,
              @userId,
              @passwordSalt
            );";
                    using var authCommand = new MySqlCommand(addToauth, connection, transaction);
                    authCommand.Parameters.AddWithValue("@passwordHash",passwordHash);
                    authCommand.Parameters.AddWithValue("@lastLoginAt", DateTime.Now);
                    authCommand.Parameters.AddWithValue("@roleId", 2);
                    authCommand.Parameters.AddWithValue("@createdDate", DateTime.Now);
                    authCommand.Parameters.AddWithValue("@createdBy", "Admin");
                    authCommand.Parameters.AddWithValue("@userId", _userId);
                    authCommand.Parameters.AddWithValue("@passwordSalt", passwordSalt);


                    var rowcount = await authCommand.ExecuteNonQueryAsync();

                if (rowcount == 0) {
                   transaction.Rollback();
                    return new SignUpResponse
                    {
                        success = false,
                        message = "User not Registered."
                    };

                }

                await transaction.CommitAsync(); ;
               var didSaveandSendToken =  await saveAndSendToken(_userId.ToString(), user.Email);
                if(!didSaveandSendToken){
                    return new SignUpResponse
                    {
                        success = false,
                        message = "User created but failed to send verification email."
                    };
                }
     
                return new SignUpResponse { 
                    success= true,
                    message = "User created successfully. Please check your email to verify your account."
                };
        

        }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                await transaction.RollbackAsync();

                return new SignUpResponse
                {
                    success = false,
                    message = "An account with this email address already exists."
                };
            }

            catch {
                await transaction.RollbackAsync();
                return new SignUpResponse
                {
                    success = false,
                    message = "Some error Occured"
                };
            }

        }

   

        public async Task<int?> refreshToken(string userID)
        {
            int? userIdFromDb;
            using var connection = _connectionFactory.GetConnection();
            await connection.OpenAsync();

            string checkuser = @"SELECT id FROM users
            WHERE id = @userId;";

            using var command = new MySqlCommand(checkuser, connection);
            command.Parameters.AddWithValue("@userId", userID);
            var  reader = await command.ExecuteReaderAsync();


            if (await reader.ReadAsync()) { 
             userIdFromDb = reader.GetInt32("id");
                if (userIdFromDb == null)
                {
                    return null;
                }
                return userIdFromDb;
            }
            return null;
             

            
        }

        public byte[] GeneratePasswordhash(string password, byte[] passwordSalt)
        {

            string passwordSaltPlusString = _configuration.GetSection("AppSettings: PasswordKey").Value +
                Convert.ToBase64String(passwordSalt);
            byte[] passwordHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8

                );

            return passwordHash;


        }

        public async Task<bool> saveAndSendToken(string userId, string email)
        {
            try
            {
                var token = _verificationService.CreateToken();
                Console.WriteLine(token);
                await _verificationService.SaveToken(userId, token);
                var verificationLink = _verificationService.GenerateConfirmationLink(token, email);
                await _emailService.sendEmail(
                     reciever: email,
                     subject: "Email Verification Required",
                     body: $@"<h1>Welcome to GPACARICOM</h1><p>Please click the link below to verify your email address:</p><a href='{verificationLink}'>Verify Email</a>"
                     );
                return true;

            }
            catch (Exception ex) {
                return false;
            }
         



        }
       
    }

}
