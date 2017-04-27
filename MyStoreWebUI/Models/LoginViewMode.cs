﻿using System.ComponentModel.DataAnnotations;

namespace MyStoreWebUI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Proszę podać nazwę użytkownika.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Proszę podać hasło.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}