import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './Services/auth.service';
import { NavbarComponent } from "./Components/navbar/navbar.component";
import { PropertiesService } from './Services/properties.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Client';
  token = localStorage.getItem('token');
  constructor(private authService:AuthService, private propertiesService:PropertiesService){}
  ngOnInit(): void {
    // this.loadproperties();
    // this.loadCurrentUserUsingToken();
  }

  loadCurrentUserUsingToken(){
    if(this.token){
      this.authService.loadCurrentUserUsingToken(this.token).subscribe({
        next:res=>{
          // console.log(res)
        },
        // error:err=>console.log(err)
      })
    }
  }

  loadproperties(){
    this.propertiesService.getProperties().subscribe({
      next:res=>console.log(res),
      error:err=>console.log(err)
    })
  }

}
