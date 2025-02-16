import { Component } from '@angular/core';
import { IGetPropery } from '../../../Models/property';
import { CommonModule } from '@angular/common';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-property-details',
  imports: [CommonModule],
  templateUrl: './property-details.component.html',
  styleUrl: './property-details.component.css'
})
export class PropertyDetailsComponent {
  property!:IGetPropery
  constructor(public bsModalRef:BsModalRef){}

  close(){
    this.bsModalRef.hide();
  }
}
