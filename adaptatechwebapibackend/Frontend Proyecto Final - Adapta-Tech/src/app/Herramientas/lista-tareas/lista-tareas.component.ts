import { Component } from '@angular/core';
import { Lista } from '../../../interfaces/lista.interface';
import { FormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthGuardService } from '../../../guards/auth-guard.service';
import { IPerfil } from '../../../interfaces/perfil.interface';
import { ICitaMedica } from '../../../interfaces/medicacion.interface';
import { CitaMedicaService } from '../../../services/cita-medica.service';
import { IAgregarCitaMedica, IModificarCitaMedica } from '../../../interfaces/citaMedica.interface';
import { da } from 'date-fns/locale';


@Component({
    selector: 'app-lista-tareas',
    standalone: true,
    imports: [FormsModule, CommonModule, RouterModule],
    templateUrl: './lista-tareas.component.html',
    styleUrls: ['./lista-tareas.component.css'],

})
export class ListaTareasComponent {
    perfil!: IPerfil;
    citaMedica: ICitaMedica[]= [];
    

    nuevaCita: IAgregarCitaMedica = {
        medico: '',
        fechaHora: new Date(), // Aquí mantenemos la estructura como en la interfaz
        centroMedico: '',
        idPerfilUsuario: 0
      };

      modificarCitaMedica: IModificarCitaMedica = {
        idCita: 0,
        medico: '',
        fechaHora: new Date(),
        centroMedico: '',
      }

      nuevaFecha: string = '';
      nuevaHora: string = '';
      nuevaFechaHora: Date = new Date();
      nuevoMedico: string = '';
      nuevoCentroMedico: string = '';


    constructor(private router: Router, private authGuardService: AuthGuardService, private citaMedicaService: CitaMedicaService) { 
        this.perfil = JSON.parse(localStorage.getItem("perfil")!) as IPerfil;
    }

    ngOnInit() : void {
        if (!this.authGuardService.isLoggedIn()) {
            this.router.navigate(['login']);
        }
        else {
            this.getCitasMedicasPerfil();
        }
    }
    getCitasMedicasPerfil(): void {
        this.citaMedicaService.getCitasMedicas(this.perfil.idPerfil).subscribe({
          next: (data) => {
            this.citaMedica = data;
            console.log('Citas Medicas obtenitas:', this.citaMedica);
          },
          error: (err) => {
            console.error('Error al obtener las citas medicas:', err);
          }
        });
      }
      agregarCita(): void {
        // Verificar si se proporciona un médico y un centro médico
        if (this.nuevoMedico.trim() === '' || this.nuevoCentroMedico.trim() === '' || this.nuevaFecha.trim() === '' 
        || this.nuevaHora.trim() === '') {
            console.error('Por favor, complete todos los campos requeridos.');
            return; // Salir del método si falta algún campo
        }
        
        const nuevaFechaHora = new Date(this.nuevaFecha + 'T' + this.nuevaHora);

        // Asignar los valores a nuevaCita
        this.nuevaCita = {
            medico: this.nuevoMedico,
            fechaHora: nuevaFechaHora,
            centroMedico: this.nuevoCentroMedico,
            idPerfilUsuario: this.perfil.idPerfil
        };
    
        // Luego envía nuevaCita al servicio
        this.citaMedicaService.agregarCitaMedica(this.nuevaCita).subscribe({
            next: (data) => {
                console.log('Cita médica agregada correctamente:', data);
                // Obtener las citas médicas actualizadas después de agregar una nueva
                this.getCitasMedicasPerfil();

                this.nuevoMedico = '';
                this.nuevaFecha = '';
                this.nuevaHora = '';
                this.nuevoCentroMedico = '';
            },
            error: (err) => {
                console.error('Error al agregar la cita médica:', err);
            }
        });
    }

    UpdateCitaMedica(cita: ICitaMedica) {
        this.modificarCitaMedica.idCita = cita.idCita;

        const nuevaFechaHora = new Date(this.nuevaFecha + 'T' + this.nuevaHora);
         // Crear un objeto de tipo IModificarCitaMedica a partir de los datos de ICitaMedica
        const citaModificada: IModificarCitaMedica = {
        idCita: cita.idCita,
        medico: cita.medico,
        fechaHora: nuevaFechaHora,
        centroMedico: cita.centroMedico
    };
        // console.log("Prueba medicacion para modificar",this.modificarMedicacion);
        // console.log("Prueba 2 medicacion para modificar",medicamento);
        this.citaMedicaService.updateCitaMedica(cita.idCita, citaModificada).subscribe({
          next: (data) => {
            console.log('Cita Medica modificada:', data);
            // Volver a cargar los medicamentos para el día actual después de modificar uno
            this.getCitasMedicasPerfil();
          },
          error: (err) => {
            console.error('Error al actualizar la cita:', err);
          }
        });
      }
    
      eliminarCitaMedica(idMedicamento: number): void {
        this.citaMedicaService.deletecitaMedica(idMedicamento).subscribe({
          next: () => {
            console.log('Cita médica eliminada exitosamente');
            // Volver a cargar los medicamentos para el día actual después de modificar uno
            this.getCitasMedicasPerfil();
            window.location.reload();
          },
          error: (error) => {
            console.error('Error al eliminar la cita:', error);
            // Aquí puedes manejar el error de acuerdo a tus necesidades
          }
        });
      }

      hoy(): Date {
        return new Date(); // Devuelve la fecha y hora actual
      }
}
