import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-settings-email',
  templateUrl: './user-settings-email.component.html',
  styleUrls: ['./user-settings-email.component.scss']
})
export class UserSettingsEmailComponent implements OnInit {

  emailModificationForm!: FormGroup;

  verificationFailedMessage = "";

  loading = false;

  constructor() {
    this.emailModificationForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email])
    });
  }

  ngOnInit(): void {

  }

  submit() {

    if (!this.emailModificationForm.valid)
      return;

      this.loading = true;

      console.log(this.emailModificationForm.value.email)
  }
}
