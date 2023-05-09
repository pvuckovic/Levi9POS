﻿namespace Levi9.POS.Domain.Common
{
    public interface IAuthenticationService
    {
        string HashPassword(string password, string salt);
        string GenerateRandomSalt(int length = 32);
    }
}
