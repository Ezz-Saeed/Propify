import { Component, OnInit } from '@angular/core';
import { IAuthUser } from '../../Models/auth.user';
import { AuthService } from '../../Services/auth.service';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { TextInputComponent } from "../text-input/text-input.component";

@Component({
  selector: 'app-edit-profile',
  imports: [ReactiveFormsModule, TextInputComponent],
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.css'
})
export class EditProfileComponent implements OnInit {
  currentUser!: IAuthUser
  editProfileForm!:FormGroup
  profileImageUrl:string | ArrayBuffer | null = null
  selectedFile?:File
  constructor(public authService:AuthService, private fb:FormBuilder){}
  ngOnInit(): void {
    this.initializeForm();
    this.loadCurrentUser();
  }

  loadCurrentUser(){
    const token = localStorage.getItem('token');
    this.authService.loadCurrentUserUsingToken(token).subscribe({
      next:res=>{
        this.currentUser = res
        // console.log(this.currentUser)
        this.profileImageUrl = this.currentUser.profileImage?.url!
        this.fillEditFormWithValues();
      },
      error:err=>console.log(err)
    })
  }

  initializeForm(){
    this.editProfileForm = this.fb.group({
      firstName:[''],
      lastName:[''],
      displayName:[''],
      profileImage:[''],
    })
  }

  fillEditFormWithValues(){
    this.editProfileForm.patchValue({
      firstName: this.currentUser.firstName,
      lastName: this.currentUser.lastName,
      displayName: this.currentUser.displayName
    });
  }

  onImageChanged(event:Event):void{
    const input = event.target as HTMLInputElement;
    if(input.files && input.files[0]){
      const file = input.files[0];
      this.selectedFile = file
      const reader = new FileReader();
      reader.onload = () =>{
        this.profileImageUrl = reader.result;
      }
      reader.readAsDataURL(file);
    }
  }

  editProfile(){
    this.authService.editProfile(this.editProfileForm, this.selectedFile).subscribe({
      next: res=>{
        console.log(res)
      },
      error:err=>console.log(err)
    })
  }

}
