import { Component, OnInit } from '@angular/core';
import {
    AbstractControl,
    FormControl,
    FormGroup,
    ValidatorFn,
    Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthResponse } from 'src/app/Models/AuthResponse';
import { Contact } from 'src/app/Models/Contact';
import { AuthenticationService } from 'src/app/services/Authentication.service';
import { AppStateService } from 'src/app/services/app-state.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
    /*
  ! remove this initialization after debugging is over !
  */
    contact: Contact = {
        firstname: 'Dan',
        lastname: 'Bravo',
        username: 'danbravo',
        emailaddress1: 'danbravo.business@loftx.net',
        contactid: '325e156236',
        statuscode: 1,
        gendercode: 1,
        jobtitle: 'ceo',
    };

    loading = false;

    genderOptions = [
      { value: 1, label: 'Male' },
      { value: 2, label: 'Female' },
    ];

    registrationForm!: FormGroup;

    // Validator 2
    assertDefault: ValidatorFn = (control: AbstractControl) => {
        const currentValue = control.value;
        if (currentValue !== this.contact.emailaddress1) {
            return { emailNotMatching: true };
        }
        return null;
    };

    // Validator 2
    passwordMatch: ValidatorFn = (control: AbstractControl) => {
        const password = control.get('password')?.value;
        const confirmPassword = control.get('confirmPassword')?.value;
        if (password !== confirmPassword) {
            return { passwordMismatch: true };
        }
        return null;
    };

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private auth: AuthenticationService,
        private appState: AppStateService
    ) {
        this.registrationForm = new FormGroup({
            firstName: new FormControl('', Validators.required),
            lastName: new FormControl('', Validators.required),
            username: new FormControl('', Validators.required),
            email: new FormControl('', [
                Validators.required,
                Validators.email,
                this.assertDefault,
            ]),
            password: new FormControl('', [
                Validators.required,
                Validators.minLength(8),
            ]),
            confirmPassword: new FormControl('', [
                Validators.required,
                this.passwordMatch,
            ]),
            jobTitle: new FormControl('', Validators.required),
            gender: new FormControl('', Validators.required),
        });
    }
    ngOnInit(): void {
        if (
            this.appState.registrationStep != 3 ||
            this.appState.contact == null
        ) {
            this.appState.registrationStep = -1;
            this.router.navigate(['']);
        }

        this.contact = this.appState.contact!;
        console.log(this.contact);
        this.registrationForm
            .get('firstName')
            ?.setValue(this.contact.firstname);
        this.registrationForm.get('lastName')?.setValue(this.contact.lastname);
        this.registrationForm.get('username')?.setValue(this.contact.username);
        this.registrationForm
            .get('email')
            ?.setValue(this.contact.emailaddress1);
        this.registrationForm.get('jobTitle')?.setValue(this.contact.jobtitle);
        this.registrationForm
            .get('gender')
            ?.setValue(this.contact.gendercode ? this.contact.gendercode : 1);
    }

    submit() {
        if (!this.registrationForm.valid) {
            return;
        }
        this.loading = true;

        this.auth
            .register(
                // passing the values
                {
                    firstname: this.registrationForm.get('firstName')!.value,
                    lastname: this.registrationForm.get('lastName')!.value,
                    username: this.registrationForm.get('username')!.value,
                    email: this.registrationForm.get('email')!.value,
                    password: this.registrationForm.get('password')!.value,
                    jobtitle: this.registrationForm.get('jobTitle')!.value,
                    gendercode: this.registrationForm.get('gender')!.value,
                    statecode: 1,
                }
            )
            .subscribe(
                // subscribing to the returned observable
                (response: AuthResponse) => {
                    // console.log(response)
                    if (response.isAuthenticated)
                        this.appState.setAuthStateLocal(response.token);

                    this.router.navigate(['']);
                },
                (error) => {
                    console.error('Error occurred while logging:', error);
                }
            );
    }
}
/*
  ! Don't forget to change the registration state back to -1 when submitting succeeds !
*/
