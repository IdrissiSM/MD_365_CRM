import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-settings-password',
  templateUrl: './user-settings-password.component.html',
  styleUrls: ['./user-settings-password.component.scss']
})
export class UserSettingsPasswordComponent implements OnInit {

  passwordModificationForm: FormGroup;

  loading = false;
  
  constructor() {
    this.passwordModificationForm = new FormGroup({
      oldPassword: new FormControl('', [Validators.required, /* !!! identity validator !!! */]),
      newPassword: new FormControl('', [Validators.required, /* !!! identity password validator !!! */]),
      newPasswordConfirmation: new FormControl('', [Validators.required, /* !!! Mismatch validator !!! */])
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

  ngOnInit(): void {
    
  }

  submit() {
    if (!this.passwordModificationForm.valid)
      return;

    this.loading = true;

    console.log(`old password: ${this.passwordModificationForm.value.oldPassword}\nnew password: ${this.passwordModificationForm.value.newPassword}`)
  }
}
