﻿namespace BlogApi.Models.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
