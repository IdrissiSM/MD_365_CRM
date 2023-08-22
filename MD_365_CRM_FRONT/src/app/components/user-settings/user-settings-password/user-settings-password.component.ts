import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { APIResponse } from 'src/app/Models/APIResponse';
import { AuthenticationService } from 'src/app/services/Authentication.service';
import { AppStateService } from 'src/app/services/app-state.service';

@Component({
  selector: 'app-user-settings-password',
  templateUrl: './user-settings-password.component.html',
  styleUrls: ['./user-settings-password.component.scss']
})
export class UserSettingsPasswordComponent implements OnInit {

  passwordModificationForm: FormGroup;

  loading = false;

  constructor(private authService: AuthenticationService, private appState: AppStateService) {
    this.passwordModificationForm = new FormGroup({
      oldPassword: new FormControl('', [Validators.required]),
      newPassword: new FormControl('', [Validators.required]),
      newPasswordConfirmation: new FormControl('', [Validators.required])
    }, {
      validators: [
        this.passwordMatch,
        this.IdentityPasswordValidator
      ]
    });
  }

  passwordMatch: ValidatorFn = (control: AbstractControl) => {
    const password = control.get('newPassword')?.value;
    const confirmPassword = control.get('newPasswordConfirmation')?.value;

    if (password !== confirmPassword) {
      return { passwordMismatch: true };
    }
    return null;
  };

  IdentityPasswordValidator: ValidatorFn = (control: AbstractControl) => {

    const password: string = control.get('newPassword')?.value;

    if (password == null || !control.touched)
      return { identityPassword: '' };

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

  ngOnInit(): void {

  }

  submit() {
    if (!this.passwordModificationForm.valid)
      return;

    this.loading = true;

    // console.log(`old password: ${this.passwordModificationForm.value.oldPassword}\nnew password: ${this.passwordModificationForm.value.newPassword}`)
    this.authService.changePassword(
      {
        email: this.appState.getAuthState().email,
        contactId: this.appState.getAuthState().contactId,
        oldPassword: this.passwordModificationForm.get('oldPassword')!.value,
        newPassword: this.passwordModificationForm.get('newPassword')!.value,
      }
    ).subscribe((response) => {
      console.log('password changed successfully!');
      this.passwordModificationForm.reset();
      this.loading = false;
    }, (error) => {
      console.log(error.errorMessages)
      this.loading = false;

    });

  }
}
