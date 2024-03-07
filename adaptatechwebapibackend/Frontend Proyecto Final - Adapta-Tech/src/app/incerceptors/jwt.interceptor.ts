import { Injectable } from "@angular/core";
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from "@angular/common/http";
import { Observable } from "rxjs";
import { ITokenInfo, IUser } from "../../interfaces/user.interface";

@Injectable({ providedIn: "root" })
export class JwtInterceptor implements HttpInterceptor {
  usuario: ITokenInfo;

  constructor() {
    this.usuario = JSON.parse(localStorage.getItem("usuario")!) as ITokenInfo;
  }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    console.log(request);

    if (this.usuario && this.usuario.token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this.usuario.token}`,
        },
      });
    }

    return next.handle(request);
  }
}
