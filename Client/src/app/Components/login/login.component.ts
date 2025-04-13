import { Component, OnInit } from '@angular/core';
import {FormsModule, FormGroup, FormBuilder, Validators, ReactiveFormsModule, } from '@angular/forms'
import { TextInputComponent } from "../text-input/text-input.component";
import { AuthService } from '../../Services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [FormsModule, ReactiveFormsModule, TextInputComponent,CommonModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  loginForm!:FormGroup
  errorMessage?: string | null = null ;
  constructor(private fb:FormBuilder, private authService:AuthService,private router:Router){}
  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    })
  }

  login() {
    this.authService.logIn(this.loginForm.value).subscribe({
      next: res => {
        this.errorMessage = null
        this.router.navigateByUrl('/properties');
      },
      error: err => {
        this.errorMessage = err.error.message
      }
    });
}

}
