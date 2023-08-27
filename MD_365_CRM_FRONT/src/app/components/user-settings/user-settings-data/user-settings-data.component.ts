import { Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { FileUpload } from 'primeng/fileupload';
import { Contact } from 'src/app/Models/Contact';
import { AuthenticationService } from 'src/app/services/Authentication.service';
import { AppStateService } from 'src/app/services/app-state.service';
import { MessageService } from 'primeng/api';



@Component({
  selector: 'app-user-settings-data',
  templateUrl: './user-settings-data.component.html',
  styleUrls: ['./user-settings-data.component.scss']
})
export class UserSettingsDataComponent implements OnInit {

  @ViewChild('fileUpload') fileUpload!: FileUpload;

  userInfoForm!: FormGroup;

  loading = false;

  profileImage: string | null = null;

  inputsDisabled = false;

  contact: Contact = {
    firstname: "",
    lastname: "",
    fullname: "",
    emailaddress1: "",
    contactid: "",
    statuscode: 1,
    gendercode: 1,
    jobtitle: "",
    secret: ""
  };

  genderOptions = [
    { value: 'Male', label: 'Male' },
    { value: 'Female', label: 'Female' },
  ];

  constructor(private appState: AppStateService, private authService: AuthenticationService, private messageService: MessageService) {
    this.userInfoForm = new FormGroup({
      firstName: new FormControl('', [Validators.required, Validators.minLength(2)]),
      lastName: new FormControl('', [Validators.required, Validators.minLength(2)]),
      username: new FormControl('', [Validators.required, Validators.minLength(6)]),
      jobTitle: new FormControl('', [Validators.required, Validators.minLength(3)]),
      gender: new FormControl('', Validators.required),
    },
      {
        validators: [
          this.dataChanged,
        ]
      });
  }

  

  ngOnInit(): void {

    this.userInfoForm.get('firstName')?.disable();
    this.userInfoForm.get('jobTitle')?.disable();
    this.userInfoForm.get('gender')?.disable();
    this.userInfoForm.get('username')?.disable();
    this.userInfoForm.get('lastName')?.disable();

    this.authService.retrieveUserData(
      {
        email: this.appState.authState.email,
      }
    ).subscribe((contact: Contact) => {

      this.userInfoForm.get('firstName')?.setValue(contact.firstname);
      this.userInfoForm.get('lastName')?.setValue(contact.lastname);
      this.userInfoForm.get('username')?.setValue(contact.fullname ?? '');
      this.userInfoForm.get('jobTitle')?.setValue(contact.jobtitle ?? '');
      this.userInfoForm.get('gender')?.setValue(contact.gendercode === 1 ? 'Male' : 'Female');

      this.appState.contact = this.contact = contact;

      console.log(this.contact);

      this.profileImage = contact.image ? URL.createObjectURL(new Blob([new Uint8Array(contact.image)], { type: 'image/png' })) : null;

      console.log(`image: ${this.profileImage}`)

      this.userInfoForm.get('firstName')?.enable();
      this.userInfoForm.get('jobTitle')?.enable();
      this.userInfoForm.get('gender')?.enable();
      this.userInfoForm.get('username')?.enable();
      this.userInfoForm.get('lastName')?.enable();


    }, (error) => {

      console.log(`failed to retrieve data`);
      console.log(error);

    })
  }

  dataChanged: ValidatorFn = (control: AbstractControl) => {

    if (
      control.get('firstName')?.value !== this.contact.firstname ||
      control.get('lastName')?.value !== this.contact.lastname ||
      control.get('username')?.value !== this.contact.fullname ||
      control.get('jobTitle')?.value !== this.contact.jobtitle ||
      control.get('gender')?.value !== (this.contact.gendercode === 1 ? 'Male' : 'Female')
    ) return null;

    return { noChange: true };
  };

  async fileToUint8Array(file: File): Promise<Uint8Array> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();

      reader.onload = (event) => {
        if (event.target && event.target.result) {
          const arrayBuffer = event.target.result as ArrayBuffer;
          const uint8Array = new Uint8Array(arrayBuffer);
          resolve(uint8Array);
        } else {
          reject(new Error('Failed to read file.'));
        }
      };

      reader.onerror = (event) => {
        reject(event.target?.error || new Error('Failed to read file.'));
      };

      reader.readAsArrayBuffer(file);
    });
  }

  async uploadImage(event: any) {
    console.log('upload image reached')
    const uploadedFile: File = event.files[0];

    this.fileUpload.clear();


    if (!uploadedFile.type.startsWith('image/')) {

      this.messageService.add({ severity: 'error', summary: 'Failed to upload', detail: 'Format unacceptable' });
      console.log('Uploaded file is not an image.');
      return;
    }

    const maxSize = 4 * 1024 * 1024; // 4 MB in bytes
    if (uploadedFile.size > maxSize) {
        this.messageService.add({ severity: 'error', summary: 'Failed to upload', detail: 'Image size exceeds 4MB' });
        console.log('Uploaded image size exceeds 4MB.');
        this.fileUpload.clear();
        return;
    }

    this.authService.addProfileImage(
      {
        email: this.appState.authState.email,
        imageData: Array.from(await this.fileToUint8Array(uploadedFile).then(uint8Array => {
          return uint8Array;
        }))
      }
    ).subscribe((response) => {
      console.log(response);
      this.messageService.add({ severity: 'success', summary: 'Profile image', detail: 'Uploaded successfully' });
      location.reload();
    }, (error) => {
      console.log(error);
    })


    // this.fileToUint8Array(uploadedFile).then(uint8Array => {
    //   console.log(URL.createObjectURL(new Blob([uint8Array.buffer], { type: 'image/png' })))
    // })
  }

  deleteAvatar() {

    this.authService.deleteProfileImage(
      {
        email: this.appState.authState.email
      }
    ).subscribe((reponse) => {
      console.log(reponse);
      location.reload();
      this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Image deleted successfully' });
    }, (error) => {
      console.log(error);
      this.messageService.add({ severity: 'error', summary: 'Failed', detail: 'Deletion failed due to internal' });
    })
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
        genderCode: this.userInfoForm.get('gender')!.value == 'Male' ? 1 : 2,
        contactId: this.contact.contactid
      }
    ).subscribe((contact) => {
      if (contact == null) {
        console.log('it is null')
        return;
      }

      this.contact = contact;
      console.log(contact);
      this.loading = false;
    }, (error) => {
      console.log(error);
    })

    console.log(this.userInfoForm.value)
  }
}
