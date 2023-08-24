import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Contact } from 'src/app/Models/Contact';
import { AuthenticationService } from 'src/app/services/Authentication.service';
import { AppStateService } from 'src/app/services/app-state.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {

  loginForm!: FormGroup;
  loading = false;

  contact: Contact = {
    firstname: "Dan",
    lastname: "Bravo",
    fullname: "danbravo",
    emailaddress1: "danbravo.business@loftx.net",
    contactid: "325e156236",
    statuscode: 1,
    gendercode: 1,
    jobtitle: "ceo",
    secret: "none"
  };

  passwordMatch: ValidatorFn = (control: AbstractControl) => {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;
    if (password !== confirmPassword) {
      return { passwordMismatch: true };
    }
    return null;
  };

  IdentityPasswordValidator: ValidatorFn = (control: AbstractControl) => {

    const password: string = control.get('password')?.value;

    if(password == null)
      return {identityPassword: ''};

    if (password.length < 8)
      return { identityPassword: 'Password must be at least 8 characters long.' };

    if (!/[a-z]/.test(password))
      return { identityPassword: 'Password must contain at least one lowercase letter.' };

    if (!/[A-Z]/.test(password))
      return { identityPassword: 'Password must contain at least one uppercase letter.' };

    if (!/[0-9]/.test(password))
      return { identityPassword: 'Password must contain at least one digit.' };

      if (!/[^a-zA-Z0-9]/.test(password))
        return { identityPassword: 'Password must contain at least one non-alphanumeric character.' };

    return null;
  };

  constructor(private auth: AuthenticationService, private router: Router, private appState: AppStateService) {

    this.loginForm = new FormGroup({
      password: new FormControl('', Validators.required),
      confirmPassword: new FormControl('', [Validators.required]),
    }, { validators: [this.passwordMatch, this.IdentityPasswordValidator ]});
  }

  ngOnInit() {
    if (this.appState.resetPasswordStep != 3) {
      this.appState.resetPasswordStep = -1;
      this.router.navigate(['']);
    }

    this.contact = this.appState.contact ?? this.contact;
  }

  submit() {
    if (this.appState.resetPasswordStep != 3)
      return;

    this.loading = true;

    this.auth.resetPassword(
      {
        email: this.contact.emailaddress1,
        password: this.loginForm.get('confirmPassword')?.value,
        secret: this.contact.secret
      }
    ).subscribe((response) => {
      console.log("reset successful")
      this.appState.registrationStep = -1;
      this.appState.resetPasswordStep = -1;
      this.router.navigate(['login']);
    }, (error) => {
      console.error('Error occurred while resetting your password:', error);
    })
  }
}
