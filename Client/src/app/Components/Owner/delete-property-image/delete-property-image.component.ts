import { Component, EventEmitter, Input } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { PropertiesService } from '../../../Services/properties.service';

@Component({
  selector: 'app-delete-property-image',
  imports: [],
  templateUrl: './delete-property-image.component.html',
  styleUrl: './delete-property-image.component.css'
})
export class DeletePropertyImageComponent {
  @Input() deletedImagePublicId = new EventEmitter();
  propertyId!:number;
  publicId!:string;

  constructor(private modalRef:BsModalRef, private propertiesService:PropertiesService){}

  deleteImageConfirmed(){
    this.propertiesService.deleteImage(this.propertyId, this.publicId).subscribe({
      next:res=>{
        this.deletedImagePublicId.emit(this.publicId);
        this.modalRef.hide()
      },
      error:err=>console.log(err)
    })
  }

  close(){
    this.modalRef.hide()
  }
}
