import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../Services/auth.service';
import { Router } from '@angular/router';
import { TextInputComponent } from "../text-input/text-input.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  imports: [FormsModule, ReactiveFormsModule, TextInputComponent,CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  registerForm!:FormGroup;
  errorMessage: string | null = null;
  constructor(private fb:FormBuilder,private authService:AuthService,
     private router:Router){}
  ngOnInit(): void {
    this.initializeRegisterForm();
  }

  initializeRegisterForm(){
    this.registerForm = this.fb.group({
      firstName:['',Validators.required],
      lastName:['',Validators.required],
      email:['',Validators.required],
      password:['',Validators.required],
      userName:['',Validators.required],
    })
  }

  register(){
    this.authService.register(this.registerForm.value).subscribe({
      next:res=>{
        // console.log(res)
        this.router.navigateByUrl('/')
      },
      error:err=>{
        this.errorMessage = err.message
      }
    })
  }

}
