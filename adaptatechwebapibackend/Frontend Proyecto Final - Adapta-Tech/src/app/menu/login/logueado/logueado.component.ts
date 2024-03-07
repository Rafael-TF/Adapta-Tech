import { Component, OnInit } from "@angular/core";
import { IPerfil, IPerfilUsuarioPut } from "../../../../interfaces/perfil.interface";
import { LoginService } from "../../../../services/login.service";
import { Router } from "@angular/router";
import { ITokenInfo } from "../../../../interfaces/user.interface";
import { AuthGuardService } from "../../../../guards/auth-guard.service";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";

@Component({
  selector: "app-logueado",
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: "./logueado.component.html",
  styleUrl: "./logueado.component.css",
})
export class LogueadoComponent {
  [x: string]: any;

  usuario = JSON.parse(localStorage.getItem("usuario")!) as ITokenInfo;

  perfil!: IPerfil;

  perfilUsuarioPut: IPerfilUsuarioPut[] = [];
  avatarUrl: string | undefined; // Ahora se declara como string | undefined
  edicionHabilitada: boolean = false; // Bandera para controlar si la edición está habilitada o no

  nuevaIdPerfil: number = 0;
  nuevaIdUsuario: number = 0;
  nuevoNombre: string = '';
  nuevosApellidos: string = '';
  nuevoTelefono: string = '';
  nuevaFechaNacimiento: Date = new Date;
  nuevoAvatar: string = '';
  nuevoAlias: string = '';

  constructor(private loginService: LoginService, private router: Router, private authGuardService: AuthGuardService) {}

  ngOnInit() {
    if (!this.authGuardService.isLoggedIn()) {
      this.router.navigate(['login']);
    } else {
      this.getProfile();
    }
  }

  getProfile() {
    this.loginService.getProfile(this.usuario.email).subscribe({
      next: (data) => {
        localStorage.setItem("perfil", JSON.stringify(data));
        this.perfil = JSON.parse(localStorage.getItem("perfil")!) as IPerfil;
        if (this.perfil.avatar) {
          this.avatarUrl = this.perfil.avatar.toString();
        } else {
          this.avatarUrl = undefined;
        }
      },
      error: (err: any) => {
        alert("No se pudo cargar el perfil");
      },
      complete: () => {},
    });
  }
  guardarCambios(): void {
    let avatarModificado: File | undefined = undefined;

    if (typeof this.nuevoAvatar === 'string') {
        // Si this.nuevoAvatar es una cadena, asumimos que es una URL y no un archivo,
        // por lo tanto, no asignamos nada a avatarModificado
    } else if (this.nuevoAvatar !== undefined && 'name' in this.nuevoAvatar) {
        // Verifica si this.nuevoAvatar es un objeto definido y tiene una propiedad 'name',
        // lo cual es característico de los objetos File
        avatarModificado = this.nuevoAvatar as File;
    }

    // Verifica si se ha proporcionado un nuevo avatar
    if (avatarModificado !== undefined) {
        // Si se ha proporcionado un nuevo avatar, actualiza el perfil con el avatar modificado
        const perfilModificado: IPerfilUsuarioPut = {
            idPerfil: this.perfil.idPerfil,
            idUsuario: this.perfil.usuarioId,
            nombre: this.nuevoNombre || this.perfil.nombre,
            apellidos: this.nuevosApellidos || this.perfil.apellidos,
            telefono: this.nuevoTelefono || this.perfil.telefono,
            fechaNacimiento: this.nuevaFechaNacimiento || this.perfil.fechaNacimiento,
            alias: this.perfil.alias,
            avatar: avatarModificado
        };

        // Llama al servicio para actualizar el perfil del usuario
        this.loginService.updatePerfilUsuario(this.perfil.idPerfil, perfilModificado).subscribe({
            next: (data) => {
                console.log('Perfil del usuario actualizado:', data);
                this.getProfile(); // Vuelve a obtener el perfil del usuario después de actualizarlo
                this.edicionHabilitada = false; // Deshabilita la edición después de guardar cambios
            },
            error: (error) => {
                console.error('Error al actualizar el perfil del usuario:', error);
            }
        });
    } else {
        // Si no se ha proporcionado un nuevo avatar, no actualices el avatar en el perfil
        const perfilModificadoSinAvatar: IPerfilUsuarioPut = {
            idPerfil: this.perfil.idPerfil,
            idUsuario: this.perfil.usuarioId,
            nombre: this.nuevoNombre || this.perfil.nombre,
            apellidos: this.nuevosApellidos || this.perfil.apellidos,
            telefono: this.nuevoTelefono || this.perfil.telefono,
            fechaNacimiento: this.nuevaFechaNacimiento || this.perfil.fechaNacimiento,
            alias: this.perfil.alias,
            avatar: this.perfil.avatar // Mantén el avatar actual sin cambios
        };

        // Llama al servicio para actualizar el perfil del usuario
        this.loginService.updatePerfilUsuario(this.perfil.idPerfil, perfilModificadoSinAvatar).subscribe({
            next: (data) => {
                console.log('Perfil del usuario actualizado sin cambiar el avatar:', data);
                this.getProfile(); // Vuelve a obtener el perfil del usuario después de actualizarlo
                this.edicionHabilitada = false; // Deshabilita la edición después de guardar cambios
            },
            error: (error) => {
                console.error('Error al actualizar el perfil del usuario sin cambiar el avatar:', error);
            }
        });
    }
}

eliminarPerfil(): void {
  if (confirm("¿Estás seguro de que quieres eliminar tu perfil? Todos tus datos serán borrados permanentemente.")) {
    console.log("Prueba eliminacion", this.perfil);
      // Si el usuario confirma la eliminación del perfil, llama a la función para eliminar el perfil
      this.loginService.deletePerfilUsuario(this.perfil.idPerfil).subscribe({
          next: () => {
              console.log('Perfil eliminado exitosamente');
              // Redirige a la página de inicio o a otra página deseada después de eliminar el perfil
              this.router.navigate(['/inicio']);
          },
          error: (error) => {
              console.error('Error al eliminar el perfil:', error);
              // Aquí puedes manejar el error de acuerdo a tus necesidades
          }
      });
  }
}

  habilitarEdicion() {
    this.edicionHabilitada = true;
    console.log("Edición de perfil habilitada");
  }

  alternarEdicion(): void {
    // Si la edición está habilitada, se guardan los cambios y se deshabilita la edición
    if (this.edicionHabilitada && this.perfil.fechaNacimiento !== null) {
      this.nuevaFechaNacimiento = this.perfil.fechaNacimiento;
        this.guardarCambios(); // Guarda los cambios en la base de datos
    } else {
        // Si la edición no está habilitada, se habilita la edición
        this.edicionHabilitada = true;
    }
  }
}