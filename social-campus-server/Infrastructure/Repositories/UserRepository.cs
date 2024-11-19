﻿using Domain.Abstractions.Repositories;
using Domain.Models.RefreshTokenModel;
using Domain.Models.UserModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        public async Task AddAsync(string login, string passwordHash, string email, string firstName, string lastName)
        {
            User newUser = new(login, passwordHash, email, firstName, lastName);
            await context.Users.AddAsync(newUser);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByRefreshTokenIdAsync(RefreshTokenId refreshTokenId)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.RefreshTokenId == refreshTokenId);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsExistByIdAsync(UserId userId)
        {
            return await context.Users.AnyAsync(u => u.Id == userId);
        }

        public async Task<bool> IsLoginUniqueAsync(string login)
        {
            return !await context.Users.AnyAsync(u => u.Login == login);
        }
    }
}
