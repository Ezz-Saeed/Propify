import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './Services/auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Client';
  token = localStorage.getItem('token');
  constructor(private authService:AuthService){}
  ngOnInit(): void {
    
    this.loadCurrentUserUsingToken();
  }

  loadCurrentUserUsingToken(){
    if(this.token){
      this.authService.loadCurrentUserUsingToken(this.token).subscribe({
        next:res=>{
          console.log(res)
        },
        error:err=>console.log(err)
      })
    }
  }

}
