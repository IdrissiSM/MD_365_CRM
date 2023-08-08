import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/Authentication.service';
import { AppStateService } from 'src/app/services/app-state.service';

@Component({
  selector: 'app-email-verification',
  templateUrl: './email-verification.component.html',
  styleUrls: ['./email-verification.component.scss']
})
export class EmailVerificationComponent implements OnInit {

  verificationForm!: FormGroup;

  loading =false;

  constructor(private route: ActivatedRoute, private auth: AuthenticationService, private router: Router, private appState: AppStateService) {

    this.verificationForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email])
    });
  }

  ngOnInit(): void {
    if(this.appState.registrationStep != 1 && this.appState.resetPasswordStep != 1) {
      this.appState.registrationStep = -1;
      this.appState.resetPasswordStep = -1;
      this.router.navigate(['']);
    }
  }

  submit() {
    if(!this.verificationForm.valid)
      return;

    this.loading = true;

    this.auth.emailVerification(
      {
        email: this.verificationForm.get('email')!.value
      }
    ).subscribe(
      () => {
        console.log("verification email sent successfully!")
        if(this.appState.registrationStep == 1) this.appState.registrationStep = 2;
        if(this.appState.resetPasswordStep == 1) this.appState.resetPasswordStep = 2;
        this.router.navigate(['verification-code'], { queryParams: { email: this.verificationForm.get('email')!.value } });
      },
      (error) => {
        console.log(error);
      }
    )
  }

}
