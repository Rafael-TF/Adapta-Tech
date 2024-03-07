import { Injectable } from "@angular/core";
import { environment } from "../environments/environment.development";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { IRegister } from "../interfaces/register.interface";

@Injectable({
  providedIn: "root",
})
export class RegisterService {
  urlAPI = environment.urlAPI;

  constructor(private http: HttpClient) {}

  register(registro: IRegister): Observable<any> {
    const formData = new FormData();
    formData.append("email", registro.email);
    formData.append("password", registro.password);
    console.log(registro);

    formData.append("avatarDTO", registro.avatar!);
    formData.append("nombre", registro.perfilUsuario.nombre);
    formData.append("apellidos", registro.perfilUsuario.apellidos);
    formData.append("telefono", registro.perfilUsuario.telefono);
    formData.append("alias", registro.perfilUsuario.alias);
    formData.append(
      "fechaNacimiento",
      registro.perfilUsuario.fechaNacimiento?.toString()!
    );

    //formData.append("perfilUsuario", JSON.stringify(registro.perfilUsuario));
    return this.http.post(this.urlAPI + "/Registro/register", formData);
  }
}