import { Injectable, OnInit } from '@angular/core';
import jwt_decode from 'jwt-decode';

@Injectable({
    providedIn: 'root',
})
export class AppStateService {
    constructor() {
        this.getAuthState();
    }

    authState!: any;

    getAuthState() {
        const authStateString = localStorage.getItem('authState');
        this.authState =
            authStateString && // get the authStateString if not null pass to the next condition
            new Date(JSON.parse(authStateString!).expiresOn).getTime() >
                new Date().getTime() // verify if it had expired
                ? JSON.parse(authStateString)
                : {
                      isAuthenticated: false,
                  };
        return this.authState;
    }

    setAuthState(token: string) {
        const decodedJWT: any = jwt_decode(token);
        this.authState = {
            isAuthenticated: true,
            uid: decodedJWT.uid,
            username: decodedJWT.sub,
            roles: decodedJWT.roles,
            token: token,
            expiresOn: new Date(decodedJWT.exp * 1000),
        };
        localStorage.removeItem('authState');
        localStorage.setItem('authState', JSON.stringify(this.authState));
    }
}
