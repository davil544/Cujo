﻿namespace CujoPasswordManager.DataModels
{
    public class Vault
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string URL { get; set; }
        public string Notes { get; set; }
    }
}