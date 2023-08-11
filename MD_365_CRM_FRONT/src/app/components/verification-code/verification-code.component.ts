import { Component, ViewChild, ElementRef, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Contact } from 'src/app/Models/Contact';
import { AuthenticationService } from 'src/app/services/Authentication.service';
import { AppStateService } from 'src/app/services/app-state.service';

@Component({
  selector: 'app-verification-code',
  templateUrl: './verification-code.component.html',
  styleUrls: ['./verification-code.component.scss']
})
export class VerificationCodeComponent implements OnInit{

  @ViewChild('input1') input1!: ElementRef<HTMLInputElement>;
  @ViewChild('input2') input2!: ElementRef<HTMLInputElement>;
  @ViewChild('input3') input3!: ElementRef<HTMLInputElement>;
  @ViewChild('input4') input4!: ElementRef<HTMLInputElement>;
  @ViewChild('input5') input5!: ElementRef<HTMLInputElement>;
  @ViewChild('input6') input6!: ElementRef<HTMLInputElement>;

  verificationForm!: FormGroup;
  email!: string;
  loading = false;

  constructor(private route: ActivatedRoute, private auth: AuthenticationService, private router: Router, private appState: AppStateService) {


    this.verificationForm = new FormGroup({
      input1: new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength(1), Validators.pattern(/^\d$/)]),
      input2: new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength(1), Validators.pattern(/^\d$/)]),
      input3: new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength(1), Validators.pattern(/^\d$/)]),
      input4: new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength(1), Validators.pattern(/^\d$/)]),
      input5: new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength(1), Validators.pattern(/^\d$/)]),
      input6: new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength(1), Validators.pattern(/^\d$/)]),

    });
  }

ngOnInit(): void {
  this.route.queryParams.subscribe( params => {
      if(params && params['email'] && (this.appState.registrationStep == 2 || this.appState.resetPasswordStep == 2)) {
        this.email = params['email'];
      } else {
        console.log("no right was given to you to access this page");
        this.appState.registrationStep = -1;
        this.appState.resetPasswordStep = -1;
        this.router.navigate(['']);
      }
    }
  )
}


  onInput(event: Event, currentInputEvent: number) {
    const target = event.target as HTMLInputElement;
    const currentValue = target.value;

    if(!['1', '2', '3', '4', '5', '6', '7', '8', '9'].includes(currentValue)) {
      this.verificationForm.get('input'+currentInputEvent)?.setValue('');
      event.preventDefault();
      return;
    }

    const subjects: {[key: string]: () => void} = {
      '2': () => this.input2.nativeElement.focus(),
      '3': () => this.input3.nativeElement.focus(),
      '4': () => this.input4.nativeElement.focus(),
      '5': () => this.input5.nativeElement.focus(),
      '6': () => this.input6.nativeElement.focus(),
    }

    if (currentValue.length === 1) subjects[(currentInputEvent+1).toString()]()
  }

  onPaste(event: ClipboardEvent) {
    event.preventDefault();
    // console.log("oncopy here!")
    const clipboardData = event.clipboardData;
    if (!clipboardData) {
      return;
    }

    const copiedValue = clipboardData.getData('text')
    // console.log(`copied data: ${copiedValue}`);
    if (!copiedValue) {
      return;
    }

    // Remove non-numeric characters from the copied value
    const numbersOnly = copiedValue.replace(/\D/g, '');
    const numberArray = numbersOnly.split('').slice(0, 6); // Take up to 6 numbers

    // console.log(`number array: ${numberArray}`)

    // console.log(`resulting array: ${numberArray}`)

    const inputFields: {[key: number]: (num: number) => void} = {
      1: (num) => this.verificationForm.get('input1')!.setValue(num),
      2: (num) => this.verificationForm.get('input2')!.setValue(num),
      3: (num) => this.verificationForm.get('input3')!.setValue(num),
      4: (num) => this.verificationForm.get('input4')!.setValue(num),
      5: (num) => this.verificationForm.get('input5')!.setValue(num),
      6: (num) => this.verificationForm.get('input6')!.setValue(num),
    }

    for(let i = 1; i<Math.min(7, numberArray.length+1); i++){
      inputFields[i](Number(numberArray[i-1]));
    }
  }

  onInputFocus(event: Event, target: number) {
    const subjects: {[key: string]: () => void} = {
      '1': () => this.verificationForm.get('input1')!.setValue(''),
      '2': () => this.verificationForm.get('input2')!.setValue(''),
      '3': () => this.verificationForm.get('input3')!.setValue(''),
      '4': () => this.verificationForm.get('input4')!.setValue(''),
      '5': () => this.verificationForm.get('input5')!.setValue(''),
      '6': () => this.verificationForm.get('input6')!.setValue(''),
    }

    subjects[target.toString()]()
  }

  assembleOtp(): number {
    let assembledOtp = '';
    for(let i = 1; i<7; i++) {
      assembledOtp += this.verificationForm.get('input'+i)!.value;
    }
    return Number(assembledOtp);
  }

  submit() {
    if(!this.verificationForm.valid)
      return;

    this.loading = true;

    this.auth.emailConfirmation(
      {
        email: this.email,
        otp: this.assembleOtp()
      }
    ).subscribe(
      (contact: Contact | null) => {
        if(contact == null) {
          console.log("your otp has expired!");
          this.router.navigate(['email-verification']);
          this.appState.registrationStep == 2 ?this.appState.registrationStep = 1: this.appState.resetPasswordStep = 1;
          return
        }
        console.log("verification complete!");
        this.appState.contact = contact;

        this.appState.registrationStep == 2? this.appState.registrationStep = 3 : this.appState.resetPasswordStep = 3;
        // console.log(contact);
        this.appState.registrationStep == 3? this.router.navigate(['register']) : this.router.navigate(['reset-password']);
      },
      (error) => {
        console.log("something went wrong while verifying your email"+error);
      }
    )
  }
}
