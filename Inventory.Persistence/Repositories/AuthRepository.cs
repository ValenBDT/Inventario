using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Entities;
using Inventory.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Repositories
{
    public class AuthRepository : IAuthRepository
    {   
        private readonly AuthContext _context;

        public AuthRepository(AuthContext context)
        {
            _context = context;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if(user is null) return null;
            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null;
            return user; 
        }

        public async Task<bool> UserExist(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if(user is not null) return true;
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt){
            using (var hmac = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt){
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)){
                var ComputeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for(int i = 0; i < ComputeHash.Length; i++){
                    if(ComputeHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }
    }
}