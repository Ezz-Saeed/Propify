import { animate, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@Component({
  selector: 'app-sidebar',
  imports: [CommonModule, ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
  animations:[
    trigger('toggleList', [
      transition(':enter', [
        style({ height: 0, opacity: 0 }),
        animate('300ms ease-out', style({ height: '*', opacity: 1 }))
      ]),
      transition(':leave', [
        animate('300ms ease-in', style({ height: 0, opacity: 0 }))
      ])
    ])
  ]
})
export class SidebarComponent {
  accountMenuOpen = false;

  toggleAccountMenu(){
    this.accountMenuOpen = !this.accountMenuOpen;
  }
}
