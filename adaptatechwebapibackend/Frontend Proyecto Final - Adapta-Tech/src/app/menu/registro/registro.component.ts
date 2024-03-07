import { Component } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { Router, RouterModule } from "@angular/router";
import { IRegister } from "../../../interfaces/register.interface";
import { RegisterService } from "../../../services/register.service";
import { LoginService } from "../../../services/login.service";
import { IUser } from "../../../interfaces/user.interface";
import { CommonModule } from "@angular/common";


@Component({
  selector: "app-registro",
  standalone: true,
  imports: [RouterModule, FormsModule, CommonModule],
  templateUrl: "./registro.component.html",
  styleUrl: "./registro.component.css",
})
export class RegistroComponent {
  registro: IRegister = {
    email: "",
    password: "",
    avatar: undefined,
    perfilUsuario: {  // Añade la información del perfil de usuario aquí
      nombre: "",
      apellidos: "",
      telefono: "",
      fechaNacimiento: null,
      alias: ""
    }
  };

  constructor(private registerService: RegisterService, private loginService: LoginService ,private router: Router) {}

  ngOnInit() {}

  onFileChange(event: any) {
    this.registro.avatar = event.target.files[0];
    console.log(event);
  }

  register() {
    this.registerService.register(this.registro).subscribe({
      next: (data) => {
        // Aquí asumimos que el servicio de registro devuelve el usuario registrado
        const usuario: IUser = {
          email: this.registro.email,
          password: this.registro.password,
        }; 
        // Iniciamos sesión con el usuario registrado
        this.loginService.login(usuario).subscribe({
          next: (data) => {
            // Aquí asumimos que el servicio de inicio de sesión devuelve el token de autenticación
            // y lo guardamos en el almacenamiento local.
            localStorage.setItem('usuario', JSON.stringify(data));
  
            // Redirigimos al usuario a la página principal después del registro y inicio de sesión.
            this.router.navigateByUrl("login");
          },
          error: (err) => {
            alert("No se ha podido iniciar sesión");
          }
        });
      },
      error: (err) => {
        alert("No se ha podido realizar el registro");
      }
    });
  }
}
