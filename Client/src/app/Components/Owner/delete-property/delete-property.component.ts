import { Component, EventEmitter, Input } from '@angular/core';
import { PropertiesService } from '../../../Services/properties.service';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-delete-property',
  imports: [],
  templateUrl: './delete-property.component.html',
  styleUrl: './delete-property.component.css'
})
export class DeletePropertyComponent {
  @Input() deletedPropertyIndex = new EventEmitter();
  propertyId!:number;
  propertyIndex!:number
  constructor(private modalRef:BsModalRef, private propertyService:PropertiesService){}

  deletionConfirmed(){
    this.propertyService.deleteProperty(this.propertyId).subscribe({
      next:res=>{
        this.deletedPropertyIndex.emit(this.propertyIndex);
        this.modalRef.hide();
      },
      error:err=>console.log(err),
    })
  }

  close(){
    this.modalRef.hide();
  }
}
