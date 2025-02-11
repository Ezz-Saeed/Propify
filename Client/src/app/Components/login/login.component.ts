import { Component, OnInit } from '@angular/core';
import {FormsModule, FormGroup, FormBuilder, Validators, ReactiveFormsModule, } from '@angular/forms'
import { TextInputComponent } from "../text-input/text-input.component";
import { AuthService } from '../../Services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [FormsModule, ReactiveFormsModule, TextInputComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  loginForm!:FormGroup
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

  login(){
    this.authService.logIn(this.loginForm.value).subscribe({
      next:res=>{
        console.log(res)
        this.router.navigateByUrl('/')
      },
      error:err=>console.log(err)
    })
  }
}
