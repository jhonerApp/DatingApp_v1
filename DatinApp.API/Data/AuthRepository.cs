using System;
using System.Threading.Tasks;
using DatinApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatinApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        public DataContext _context { get; }
        public AuthRepository(DataContext context)
        {
            this._context = context;
        }

        async Task<tbl_user> IAuthRepository.Login(string username, string password)
        {

            var user = await _context.tbl_user.FirstOrDefaultAsync(x => x.Username == username);

            
            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {

            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {

                var computerHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < computerHash.Length; i++)
                {
                    if(computerHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {

                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }

          
        }

        async Task<tbl_user> IAuthRepository.Register(tbl_user user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.tbl_user.AddAsync(user);
            await _context.SaveChangesAsync();
           

            return user;
        }

        async Task<bool> IAuthRepository.UserExist(string username)
        {
            if(await _context.tbl_user.AnyAsync(x => x.Username == username))
                return true;

            return false;
        }
    }
}