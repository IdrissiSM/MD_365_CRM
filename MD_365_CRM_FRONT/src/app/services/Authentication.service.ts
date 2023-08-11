import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AppStateService } from './app-state.service';
import { Login } from '../Models/login';
import { Register } from '../Models/register';
import { AuthResponse } from '../Models/AuthResponse';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class AuthenticationService {
    apiUrl = environment.apiBaseUrl;

    constructor(private http: HttpClient, private appState: AppStateService) {}

    login(loginRequest: Login): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(
            `${this.apiUrl}/Auth/login`,
            loginRequest
        );
    }

    register(registerRequest: Register): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(
            `${this.apiUrl}/Auth/register`,
            registerRequest
        );
    }
}
