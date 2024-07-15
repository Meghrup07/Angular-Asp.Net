import { CommonModule, JsonPipe } from '@angular/common';
import { Component, inject, OnInit, output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { DatePickerComponent } from "../_forms/date-picker/date-picker.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, TextInputComponent, DatePickerComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  private accountServices = inject(AccountService);
  private router = inject(Router);
  private fb = inject(FormBuilder);
  cancelRegister = output<boolean>();
  registerForm: FormGroup = new FormGroup({});
  maxDate = new Date();
  validationErrors: string[] | undefined;

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchVales('password')]],
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }

  matchVales(machTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value == control.parent?.get(machTo)?.value
        ? null : { isMatching: true }
    }
  }

  register() {
    const dob = this.getDateOnly(this.registerForm.get('dateOfBirth')?.value);
    this.registerForm.patchValue({ dateOfBirth: dob });
    this.accountServices.register(this.registerForm.value).subscribe({
      next: response => {
        this.router.navigateByUrl('/members')
      },
      error: error => this.validationErrors = error
    });
  }

  cancel() {
    this.cancelRegister.emit(false)
  }

  private getDateOnly(dob: string | undefined) {
    if (!dob) return;
    return new Date(dob).toISOString().slice(0, 10);
  }

}
