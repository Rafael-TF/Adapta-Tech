import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TextoService } from '../../../services/texto.service';
import { ITestimonio, ITestimonioPost, ITestimonioPut, ITestimoniosGet } from '../../../interfaces/testimonio.interface';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TestimonioService } from '../../../services/testimonio.service';
import { AuthGuardService } from '../../../guards/auth-guard.service';
import { IPerfil } from '../../../interfaces/perfil.interface';

@Component({
  selector: 'app-testimonio',
  standalone: true,
  imports: [RouterModule, FormsModule, CommonModule],
  templateUrl: './testimonio.component.html',
  styleUrl: './testimonio.component.css'
})
export class TestimonioComponent implements OnInit {
  perfil!: IPerfil;
  getTestimonios: ITestimoniosGet[] = []; 
  postTestimonios: ITestimonioPost[] = [];
  putTestimonios: ITestimonioPut[] = [];
  testimonios: ITestimonio[] = [];
  estaLogueado: boolean = false;
  testimonioEnEdicionId: number | null = null;
  edicionHabilitada: boolean = false; // Bandera para controlar si la edición está habilitada o no

  // Objeto para almacenar el estado de edición de cada testimonio por su id
  testimonioEnEdicion: { [id: number]: boolean } = {};


  nuevoTestimonio: ITestimonioPost = {
    IdPerfilUsuario: 0,
    TituloTestimonio: '',
    TextoTestimonio: ''
  }

  testimonioModificado: ITestimonioPut = {
    idTestimonio: 0,
    tituloTestimonio: '',
    textoTestimonio: ''
  }

  nuevoTituloTestimonio: string = '';
  nuevoTextoTestimonio: string = '';
  
  constructor(private textoService: TextoService, private testimonioService: TestimonioService, private authGuardService: AuthGuardService) {
      // this.estaLogueado = authGuardService.isLoggedIn();
      this.actualizarEstadoLogueado();
      this.perfil = JSON.parse(localStorage.getItem("perfil")!) as IPerfil;
   }
   actualizarEstadoLogueado(): void {
    this.estaLogueado = this.authGuardService.isLoggedIn();
}
  ngOnInit(): void {
    this.testimonioService.obtenerTestimonios().subscribe(testimonios => {
      this.testimonios = testimonios;
      this.mostrarTestimonios();  
    });
  }
  
  mostrarTestimonios(): void {
    this.testimonioService.getTestimonios().subscribe({
      next: (data) => {
        this.getTestimonios = data.map(testimonio => ({ ...testimonio, enEdicion: false }));
        // console.log('Testimonios obtenidos:', this.getTestimonios);
      },
      error: (err) => {
        console.error('Error al obtener los medicamentos:', err);
      }
    });
  }

  obtenerTamanoTexto(): number {
    return this.textoService.getTamanoTexto();
  }
  addTestimonio(formulario: NgForm): void {
    if (formulario.valid) {
      this.nuevoTestimonio = {
        IdPerfilUsuario: this.perfil.idPerfil,
        TituloTestimonio: this.nuevoTestimonio.TituloTestimonio,
        TextoTestimonio: this.nuevoTestimonio.TextoTestimonio
      };
      this.testimonioService.addTestimonio(this.nuevoTestimonio).subscribe({
        next: (data) => {
          this.nuevoTituloTestimonio = '';
          this.nuevoTextoTestimonio = '';
          this.mostrarTestimonios();
          formulario.resetForm();
        },
        error: (err) => {
          console.error('Error al añadir el testimonio:', err);
        }
      });
    }
  }
  agregarTestimonio(testimonio: ITestimonio) {
    this.testimonios.push({...testimonio});
    // console.log(this.testimonios);
  }


  editarTitulo(testimonio: ITestimoniosGet): void {
    // Verificar si el usuario actual es el propietario del testimonio
    if (testimonio.idPerfilUsuario === this.perfil.idPerfil) {
        this.testimonioEnEdicionId = testimonio.idTestimonio;
        // console.log("Prueba Id para el titulo", this.testimonioEnEdicionId);
    } else {
        // console.log("No tienes permiso para editar este testimonio.");
    }
}

editarTexto(testimonio: ITestimoniosGet): void {
    // Verificar si el usuario actual es el propietario del testimonio
    if (testimonio.idPerfilUsuario === this.perfil.idPerfil) {
        this.testimonioEnEdicionId = testimonio.idTestimonio;
        console.log("Prueba Id para el texto", this.testimonioEnEdicionId);
    } else {
        // console.log("No tienes permiso para editar este testimonio.");
    }
}

  guardarCambios(testimonio: ITestimonioPut): void {
    const testimonioId = testimonio.idTestimonio;
    this.testimonioEnEdicion[testimonioId] = false; // Desactivar la edición
    // console.log(testimonioId);
    // console.log('Guardando cambios para el testimonio:', testimonio);
    this.testimonioService.updateTestimonio(testimonio.idTestimonio, this.perfil.idPerfil, testimonio).subscribe({
      next: (data) => {
        // console.log('Testimonio modificado:', data);
        this.mostrarTestimonios();
        this.testimonioEnEdicionId = null; // Limpiar la variable después de guardar los cambios
      },
      error: (err) => {
        console.error('Error al modificar el testimonio:', err);
      }
    });
  }

  eliminarTestimonio(idTestimonio: number, idPerfilUsuario: number): void {
    this.testimonioService.deleteTestimonio(idTestimonio, idPerfilUsuario).subscribe({
      next: () => {
        // console.log('Testimonio eliminado exitosamente');
        // Volver a cargar los medicamentos para el día actual después de modificar uno
        this.mostrarTestimonios();
      },
      error: (error) => {
        console.error('Error al eliminar el medicamento:', error);
      }
    });
  }

  alternarEdicion(testimonio: ITestimoniosGet): void {
    // Verificar si el testimonio actual está en modo de edición
    if (this.testimonioEnEdicionId === testimonio.idTestimonio) {
        // Si ya está en modo de edición, guardar los cambios
        this.guardarCambios(testimonio);
    } else {
        // Si no está en modo de edición, habilitar la edición
        this.testimonioEnEdicionId = testimonio.idTestimonio;
        console.log("Edición habilitada para el testimonio:", testimonio.idTestimonio);
    }
} 
}
