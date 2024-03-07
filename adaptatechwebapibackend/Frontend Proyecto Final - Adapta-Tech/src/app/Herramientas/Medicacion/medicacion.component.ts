import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { IPerfil } from '../../../interfaces/perfil.interface';
import { IAnnadirMedicacion, IMedicacion, IModificarMedicamento } from '../../../interfaces/medicacion.interface';
import { MedicacionService } from '../../../services/medicacion.service';
import { AuthGuardService } from '../../../guards/auth-guard.service';

@Component({
  selector: 'app-medicacion',
  standalone: true,
  imports: [RouterModule, FormsModule, CommonModule],
  templateUrl: './medicacion.component.html',
  styleUrl: './medicacion.component.css',
  
})
export class MedicacionComponent{
  perfil!: IPerfil;
  medicacion: IMedicacion[] = [];
  medicacionPorDia: IMedicacion[] = [];
  diaSeleccionado: string | undefined;
  medicacionModificada: IModificarMedicamento[] = [];
  editarCampos: boolean = false;
  medicamentoSeleccionado: IMedicacion | null = null;
  medicacionFiltradaPorId: IModificarMedicamento[] = [];

  nuevaMedicacion: IAnnadirMedicacion = {
    medicacion: '',
    posologia: '',
    funcion: '',
    diaSemana: '',
    idPerfilUsuario: 0,
  };

  modificarMedicacion: IModificarMedicamento = {
    idMedicamento: 0,
    medicacion: '',
    posologia: '',
    funcion: '',
    diaSemana: '',
  }

  nombreNuevoMedicamento: string = '';
  nuevaPosologiaMedicamento: string = '';
  nuevaFuncionMedicamento: string = '';
  contadorMedicamentos: number = 1;
  idMedicamento: number | undefined;
 

  constructor(private medicacionService: MedicacionService, private router: Router, private authGuardService: AuthGuardService) {
    this.perfil = JSON.parse(localStorage.getItem("perfil")!) as IPerfil;
    this.diaSeleccionado = 'Lunes';
  }

  ngOnInit() : void {
    if (!this.authGuardService.isLoggedIn()) {
      this.router.navigate(['login']);
    } else {
      this.perfil = JSON.parse(localStorage.getItem("perfil")!) as IPerfil;
      const diaSemana = new Date().getDay();
      switch (diaSemana) {
        case 0:
        this.cambiarDia ('Domingo');
        (document.getElementById('tab-seven') as HTMLInputElement).checked = true;
        break;
        case 1:
        this.cambiarDia ('Lunes');
        (document.getElementById('tab-one') as HTMLInputElement).checked = true;
        break;
        case 2:
        this.cambiarDia ('Martes');
        (document.getElementById('tab-two') as HTMLInputElement).checked = true;
        break;
        case 3:
        this.cambiarDia ('Miércoles');
        (document.getElementById('tab-three') as HTMLInputElement).checked = true;
        break;
        case 4:
        this.cambiarDia ('Jueves');
        (document.getElementById('tab-four') as HTMLInputElement).checked = true;
        break;
        case 5:
        this.cambiarDia ('Viernes');
        (document.getElementById('tab-five') as HTMLInputElement).checked = true;
        break;
        case 6:
        this.cambiarDia ('Sábado');
        (document.getElementById('tab-six') as HTMLInputElement).checked = true;
        break;
      }
    }
  }

  cambiarDia(dia: string) { 
    this.diaSeleccionado = dia;
    this.getMedicamentosPorDia();
  }

  getMedicamentosPorPerfil(perfilId: number): void {
    this.medicacionService.getMedicaciones(this.perfil.idPerfil).subscribe({
      next: (data) => {
        this.medicacion = data;
        this.medicacionDia(this.diaSeleccionado!);
      },
      error: (err) => {
        console.error('Error al obtener los medicamentos:', err);
      }
    });
  }
  
  getMedicamentosPorDia(): void {
    this.medicacionService.getMedicamentosPorDia(this.perfil.idPerfil, this.diaSeleccionado!).subscribe({
      next: (data) => {
        this.medicacion = data;
        this.medicacionDia(this.diaSeleccionado!);
      },
      error: (err) => {
        this.medicacionPorDia = [];
        if (err.error instanceof ErrorEvent) {
          // alert("Error del cliente: " + err.error.message);
        } else {
          // alert("Error del servidor: " + err.status + " - " + err.error);
        }
      },
      complete: () => console.log("Solicitud completada"),
    });
  }
  
  medicacionDia(dia: string) {
    if (!this.medicacion || this.medicacion.length === 0) {
      // console.log("La variable 'medicacion' está vacía o no está definida.");
      return;
    }

    // console.log('Día seleccionado:', this.diaSeleccionado);
    this.medicacionPorDia = this.medicacion.filter(medicamento => medicamento.diaSemana === dia);
    // console.log('Medicamentos por día:', this.medicacionPorDia);
  }

  addMedicacion(): void {
    this.nuevaMedicacion = {
      medicacion: this.nombreNuevoMedicamento,
      posologia: this.nuevaPosologiaMedicamento,
      funcion: this.nuevaFuncionMedicamento,
      diaSemana: this.diaSeleccionado,
      idPerfilUsuario:this.perfil.idPerfil
    }
    this.medicacionService.addMedicacion(this.nuevaMedicacion ).subscribe({
      next: (data) => {
        // console.log('Medicamento añadido:', data);
        this.nombreNuevoMedicamento = '';
        this.nuevaPosologiaMedicamento = '';
        this.nuevaFuncionMedicamento = '';
        // Volver a cargar los medicamentos para el día actual después de añadir uno nuevo
        this.getMedicamentosPorDia();
      },
      error: (err) => {
        console.error('Error al añadir el medicamento:', err);
      }
    });
  }

  modificarMedicamento(medicamento: IMedicacion) {
    this.modificarMedicacion.idMedicamento = medicamento.idMedicamento;
    // console.log("Prueba medicacion para modificar",this.modificarMedicacion);
    // console.log("Prueba 2 medicacion para modificar",medicamento);
    this.medicacionService.updateMedicacion(medicamento.idMedicamento, medicamento).subscribe({
      next: (data) => {
        console.log('Medicamento actualizado:', data);
        // Volver a cargar los medicamentos para el día actual después de modificar uno
        this.getMedicamentosPorDia();
      },
      error: (err) => {
        console.error('Error al actualizar el medicamento:', err);
      }
    });
  }

  eliminarMedicamento(idMedicamento: number): void {
    this.medicacionService.deleteMedicacion(idMedicamento).subscribe({
      next: () => {
        console.log('Medicamento eliminado exitosamente');
        // Volver a cargar los medicamentos para el día actual después de modificar uno
        this.getMedicamentosPorDia();
      },
      error: (error) => {
        console.error('Error al eliminar el medicamento:', error);
        // Aquí puedes manejar el error de acuerdo a tus necesidades
      }
    });
    window.location.reload();
  }
}


