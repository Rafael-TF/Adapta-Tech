import { Injectable } from '@angular/core';
import { environment } from '../environments/environment.development';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ITokenInfo } from '../interfaces/user.interface';
import { IAnnadirMedicacion, IMedicacion, IModificarMedicamento } from '../interfaces/medicacion.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MedicacionService {
  urlAPI = environment.urlAPI;
  constructor(private http: HttpClient) { }

  getMedicaciones(idPerfilUsuario: number): Observable<IMedicacion[]> {
    const headers = this.getHeaders();
    return this.http.get<IMedicacion[]>(this.urlAPI + `/Medicamentos/porPerfilUsuario/${idPerfilUsuario}`, {
      headers: headers,
    });
  }

  getMedicamentosPorDia(idPerfilUsuario: number, dia: string): Observable<IMedicacion[]> {
    const headers = this.getHeaders();
    return this.http.get<IMedicacion[]>(this.urlAPI + `/Medicamentos/porDiaSemana/${idPerfilUsuario}/${dia}`, {
      headers: headers,
    });
  }

  getMedicamentoPorId(idMedicamento: number): Observable<IMedicacion> {
    const headers = this.getHeaders();
    return this.http.get<IMedicacion>(`${this.urlAPI}/Medicamentos/${idMedicamento}`, {
      headers: headers,
    });
  }

  addMedicacion(medicacion: IAnnadirMedicacion): Observable<any> {
    const headers = this.getHeaders();

    return this.http.post<IMedicacion>(
      this.urlAPI + "/Medicamentos/agregarMedicamento",
      medicacion,
      {
        headers: headers,
      }
    );
  }

  updateMedicacion(idMedicamento: number, medicacion: IModificarMedicamento): Observable<IModificarMedicamento> {
    const headers = this.getHeaders();

    return this.http.put<IModificarMedicamento>(
      `${this.urlAPI}/Medicamentos/modificarMedicamento/${idMedicamento}`,
      medicacion,
      {
        headers: headers,
      }
    );
  }

  deleteMedicacion(idMedicamento: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.delete<any>(`${this.urlAPI}/Medicamentos/eliminarMedicamento/${idMedicamento}`, { headers });
  }

  getHeaders(): HttpHeaders {
    const usuario = JSON.parse(localStorage.getItem("usuario")!) as ITokenInfo;
    const headers = new HttpHeaders({
      Authorization: "Bearer " + usuario.token,
      "Content-Type": "application/json",
    });
    return headers;
  }
}