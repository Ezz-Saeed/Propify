import { Component, OnInit } from '@angular/core';
import { IGetPropery } from '../../../Models/property';
import { PropertiesService } from '../../../Services/properties.service';
import { CommonModule } from '@angular/common';
import { BsModalRef, BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { UpdatePropertyComponent } from '../update-property/update-property.component';
import { DeletePropertyComponent } from '../delete-property/delete-property.component';

@Component({
  selector: 'app-properties',
  imports: [CommonModule, ModalModule],
  templateUrl: './properties.component.html',
  styleUrl: './properties.component.css',
  providers:[
    BsModalService
  ]
})
export class PropertiesComponent implements OnInit {
  public properties!:IGetPropery[];
  bsModalRef!:BsModalRef
  deleteModal!:BsModalRef
  constructor(private propertyService:PropertiesService, private bsModalService:BsModalService){}
  ngOnInit(): void {
    this.loadProperties();
  }

  loadProperties(){
    this.propertyService.getPropertiesForOwner().subscribe({
      next: res=>{
        this.properties = res
      },
      error:err=>{
        console.log(err);
      }
    })
  }

  updateForm(property:IGetPropery, i:number){
    const config = {
      class:'modal-dialog-centered modal-lg',
      initialState:{
        property,
      }
    }
    this.bsModalRef = this.bsModalService.show(UpdatePropertyComponent, config);
    this.bsModalRef.content.updatedProperty.subscribe({
      next: (p:IGetPropery)=>{
        this.properties[i] = p;
      }
    })
  }

  deleteProperty(propertyId:number, propertyIndex:number){
    const config = {
      class:'modal-dialog-centered',
      initialState:{
        propertyId,
        propertyIndex
      }
    }
    this.deleteModal = this.bsModalService.show(DeletePropertyComponent, config);
    this.deleteModal.content.deletedPropertyIndex.subscribe({
      next:(index:number)=>{
        this.properties.splice(index,1);
      }
    })
  }
}
