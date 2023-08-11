import { Component } from '@angular/core';
import {
    CheckboxControlValueAccessor,
    FormControl,
    FormGroup,
    Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthResponse } from 'src/app/Models/AuthResponse';
import { AuthenticationService } from 'src/app/services/Authentication.service';
import { AppStateService } from 'src/app/services/app-state.service';
import { LayoutService } from 'src/app/layout/service/app.layout.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
    loginForm!: FormGroup;

    constructor(
        public layoutService: LayoutService,
        private auth: AuthenticationService,
        private router: Router,
        private appState: AppStateService
    ) {
        this.loginForm = new FormGroup({
            email: new FormControl('', [Validators.required, Validators.email]), // Specify data type and validators for email
            password: new FormControl('', Validators.required), // Specify data type and required validator for password
        });
    }

    submit() {
        if (!this.loginForm.valid) return;

        this.auth
            .login({
                email: this.loginForm.get('email')!.value,
                password: this.loginForm.get('password')!.value,
            })
            .subscribe(
                (response: AuthResponse) => {
                    console.log(response.isAuthenticated);
                    console.log(response.token);
                    console.log(response.message);

                    if (response.isAuthenticated)
                        this.appState.setAuthState(response.token);

                    this.router.navigate(['']);
                },
                (error) => {
                    console.error('Error occurred while logging:', error);
                }
            );
    }
}
