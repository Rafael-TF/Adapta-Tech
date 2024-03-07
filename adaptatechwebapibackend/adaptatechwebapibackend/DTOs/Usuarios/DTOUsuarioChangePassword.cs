using System;
namespace adaptatechwebapibackend.DTOs.Usuarios
{
    public class DTOUsuarioChangePassword
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Enlace { get; set; }
    }
}

