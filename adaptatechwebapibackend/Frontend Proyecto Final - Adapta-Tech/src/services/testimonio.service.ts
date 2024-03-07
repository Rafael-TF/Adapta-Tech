import { BehaviorSubject, Observable } from "rxjs";
import { ITestimonio, ITestimonioPost, ITestimonioPut, ITestimoniosGet } from "../interfaces/testimonio.interface";
import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { ITokenInfo } from "../interfaces/user.interface";
import { environment } from "../environments/environment.development";

@Injectable({
    providedIn: 'root'
  })
  
export class TestimonioService {
    private testimonios: ITestimonio[] = [];
    private testimoniosSubject = new BehaviorSubject<ITestimonio[]>([]);
  
    urlAPI = environment.urlAPI;
    constructor(private http: HttpClient) { }
    
    agregarTestimonio(testimonio: ITestimonio) {
      this.testimonios.push(testimonio);
      this.testimoniosSubject.next(this.testimonios);
    }
    
    obtenerTestimonios(): Observable<ITestimonio[]> {
      return this.testimoniosSubject.asObservable();
    }

    getTestimonios(): Observable<ITestimoniosGet[]> {
      const headers = this.getHeaders();
      return this.http.get<ITestimoniosGet[]>(this.urlAPI + `/Testimonios/MostrarTestimonios`, {
        headers: headers,
      });
    }

    addTestimonio(testimonio: ITestimonioPost): Observable<any> {
      const headers = this.getHeaders();
  
      return this.http.post<ITestimonioPost>(
        this.urlAPI + "/Testimonios/agregarTestimonio",
        testimonio,
        {
          headers: headers,
        }
      );
    }

    updateTestimonio(idTestimonio: number, idPerfilUsuario: number, testimonio: ITestimonioPut): Observable<ITestimonioPut> {
      const headers = this.getHeaders();
    
      return this.http.put<ITestimonioPut>(
        `${this.urlAPI}/Testimonios/modificarTestimonio/${idTestimonio}/${idPerfilUsuario}`,
        testimonio,
        {
          headers: headers,
        }
      );
    }
    

    deleteTestimonio(idTestimonio: number, idPerfilUsuario: number): Observable<any> {
      const headers = this.getHeaders();
      return this.http.delete<any>(`${this.urlAPI}/Testimonios/eliminarTestimonio/${idTestimonio}/${idPerfilUsuario}`, { headers });
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
  
