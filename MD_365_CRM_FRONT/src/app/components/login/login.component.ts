import { Component } from '@angular/core';
import { CheckboxControlValueAccessor, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthResponse } from 'src/app/Models/AuthResponse';
import { AuthenticationService } from 'src/app/services/Authentication.service';
import { AppStateService } from 'src/app/services/app-state.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  loginForm!: FormGroup;



  constructor(private auth: AuthenticationService, private router: Router, private appState: AppStateService) {

    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]), // Specify data type and validators for email
      password: new FormControl('', Validators.required), // Specify data type and required validator for password
      checkbox: new FormControl(false, Validators.required)
    });
  }

  submit() {

    if(!this.loginForm.valid)
      return;

      this.auth.login( // passing the values
        {
          email: this.loginForm.get('email')!.value,
          password: this.loginForm.get('password')!.value
        }).subscribe( // subscribing to the returned observable
        (response: AuthResponse) => {

          if(response.isAuthenticated)
            if(this.loginForm.get('checkbox')!.value) this.appState.setAuthStateLocal(response.token)
            else this.appState.setAuthStateSession(response.token)
          this.router.navigate(['']);

        },
        (error) => {
          console.error('Error occurred while logging:', error);
        }
      );
  }

  InitializeRegistration() {
    this.appState.registrationStep = 1;
  }
}
