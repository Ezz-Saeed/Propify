import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './Services/auth.service';
import { NavbarComponent } from "./Components/navbar/navbar.component";
import { PropertiesService } from './Services/properties.service';
import { SidebarComponent } from "./Components/sidebar/sidebar.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, SidebarComponent, CommonModule, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit, AfterViewInit {
  @ViewChild('navbar', {static:true}) navbar!:ElementRef
  title = 'Client';
  token = localStorage.getItem('token');
  sidebarOpen =false;
  constructor(private authService:AuthService, private propertiesService:PropertiesService){}

  ngAfterViewInit(): void {
    const navbarHeight = this.navbar.nativeElement.offsetHeight;
    document.documentElement.style.setProperty('--navbar-height', `${navbarHeight}px`)
  }
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

  toggleSidebar() {
    this.sidebarOpen = !this.sidebarOpen;
  }

}
