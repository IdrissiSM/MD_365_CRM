import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Contact } from 'src/app/Models/Contact';
import { AuthenticationService } from 'src/app/services/Authentication.service';
import { AppStateService } from 'src/app/services/app-state.service';


@Component({
  selector: 'app-user-settings-data',
  templateUrl: './user-settings-data.component.html',
  styleUrls: ['./user-settings-data.component.scss']
})
export class UserSettingsDataComponent implements OnInit{

  userInfoForm!: FormGroup;

  loading = false;

  contact: Contact = {
    firstname: "Brahim",
    lastname: "BAHI",
    username: "bahiibrahim",
    emailaddress1: "brahim.bahi@ump.ac.ma",
    contactid: "789ee997-0f32-ee11-bdf4-6045bd905df9",
    statuscode: 1,
    gendercode: 1,
    jobtitle: "ceo",
    secret: "none"
  };

  genderOptions = [
    { value: 'Male', label: 'Male' },
    { value: 'Female', label: 'Female' },
  ];

  constructor(private appState: AppStateService, private authService: AuthenticationService) {
    this.userInfoForm = new FormGroup({
      firstName: new FormControl('', [Validators.required, Validators.minLength(2)]),
      lastName: new FormControl('', [Validators.required, Validators.minLength(2)]),
      username: new FormControl('', [Validators.required, Validators.minLength(6)]),
      jobTitle: new FormControl('', [Validators.required, Validators.minLength(3)]),
      gender: new FormControl('', Validators.required),
    },
    { validators: [
      this.dataChanged,
    ] });
  }

  dataChanged: ValidatorFn = (control: AbstractControl) => {

    if (
      control.get('firstName')?.value !== this.contact.firstname ||
      control.get('lastName')?.value !== this.contact.lastname ||
      control.get('username')?.value !== this.contact.username ||
      control.get('jobTitle')?.value !== this.contact.jobtitle ||
      control.get('gender')?.value !== (this.contact.gendercode === 1 ? 'Male' : 'Female')
    ) return null;

    return { noChange: true };
  };

  ngOnInit(): void {

    // this.contact = this.appState.contact;
    this.userInfoForm.get('firstName')?.setValue(this.contact.firstname);
    this.userInfoForm.get('lastName')?.setValue(this.contact.lastname);
    this.userInfoForm.get('username')?.setValue(this.contact.username ?? '');
    this.userInfoForm.get('jobTitle')?.setValue(this.contact.jobtitle ?? '');
    this.userInfoForm.get('gender')?.setValue(this.contact.gendercode === 1 ? 'Male' : 'Female');
  }

  submit() {

    if (!this.userInfoForm.valid)
      return;

    this.loading = true;

    this.authService.updateProfile(
      {
        username: this.userInfoForm.get('username')!.value,
        firstName: this.userInfoForm.get('firstName')!.value,
        lastName: this.userInfoForm.get('lastName')!.value,
        email: this.contact.emailaddress1,
        jobTitle: this.userInfoForm.get('jobTitle')!.value,
        genderCode: this.userInfoForm.get('gender')!.value == 'Male' ? 1 : 0,
        contactId: this.contact.contactid
      }
    ).subscribe((contact) => {

      this.loading = false;
      if(contact == null) {
        console.log('it is null')
        return;
      }

      this.contact = contact;
      console.log(contact);
    }, (error) => {
      console.log(error);
    })

    console.log(this.userInfoForm.value)
  }
}
