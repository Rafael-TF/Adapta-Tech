import { Component, Input } from "@angular/core";
import { IMensajeForo } from "../../../../interfaces/foro.interface";
import { CommonModule, DatePipe } from "@angular/common";
import { format } from "date-fns";
import { es } from "date-fns/locale";
import { LoginService } from "../../../../services/login.service";

@Component({
  selector: "app-table-tema",
  standalone: true,
  imports: [CommonModule],
  templateUrl: "./table-tema.component.html",
  styleUrl: "./table-tema.component.css",
})
export class TableTemaComponent {
  @Input() mensajes: IMensajeForo[] = [];

  constructor(private loginService: LoginService) {}

  formatearFecha(fecha: Date, formato: string = "dd/MM/yyyy HH:mm a"): string {
    return format(fecha, formato, { locale: es });
  }

  getAlias(id: number) {
    this.loginService.getProfileById(id).subscribe({
      next: (data) => {
        return data.alias;
      },
      error: (err: any) => {
        alert("No se pudo cargar el perfil");
      },
      complete: () => {},
    });
  }
}
