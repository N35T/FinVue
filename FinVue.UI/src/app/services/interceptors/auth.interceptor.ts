import { HttpInterceptor, HttpErrorResponse, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, of, throwError, catchError } from "rxjs";
import { AuthService } from "../auth.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    
    constructor(private authService : AuthService) { }

    private handleAuthError(err: HttpErrorResponse): Observable<any> {
        if (err.status === 401 || err.status === 403) {
            this.authService.redirectToAuth();
            return of(err.message);
        }
        return throwError(() => err);
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req)
            .pipe(catchError(x=> this.handleAuthError(x)));
    }
}