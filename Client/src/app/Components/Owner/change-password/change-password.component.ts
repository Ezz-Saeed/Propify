import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { AuthService } from '../../../Services/auth.service';
import { TextInputComponent } from "../../text-input/text-input.component";
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-change-password',
  imports: [ReactiveFormsModule, TextInputComponent, CommonModule],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css'
})
export class ChangePasswordComponent implements OnInit {
  changePasswordForm!:FormGroup
  errorMessage:string | null = null;
  constructor(private authService:AuthService, private fb:FormBuilder, private router:Router){}

  ngOnInit(): void {
    this.initializeForm();
  }

  private initializeForm(){
    this.changePasswordForm = this.fb.group({
      currentPassword:['', Validators.required],
      newPassword:['', Validators.required],
      confirmNewPassword:['', [Validators.required, this.matchPasswords('newPassword')]]
    })
  }

  private matchPasswords(matchTo:string):ValidatorFn{
    return (control:AbstractControl)=>{
      return control.value === control.parent?.get(matchTo)?.value
      ? null : {isNotMatch:true};
    }
  }

  changePassword(){
    this.authService.changPassword(this.changePasswordForm.value).subscribe({
      next:res=>{
        this.errorMessage = null
        this.router.navigateByUrl('/properties');
      },
      error:err=>{
        this.errorMessage = err.error.message;
      }
    })
  }

}
