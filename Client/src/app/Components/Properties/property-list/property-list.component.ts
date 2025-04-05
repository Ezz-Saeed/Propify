import { Component, OnInit } from '@angular/core';
import { PropertiesService } from '../../../Services/properties.service';
import { IGetPropery } from '../../../Models/property';
import { CommonModule } from '@angular/common';
import { BsModalRef, BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { PropertyDetailsComponent } from '../property-details/property-details.component';

@Component({
  selector: 'app-property-list',
  imports: [CommonModule, ModalModule],
  templateUrl: './property-list.component.html',
  styleUrl: './property-list.component.css',
  providers:[
    BsModalService
  ]
})
export class PropertyListComponent implements OnInit {
  properties:IGetPropery[] = []
  bsModalRef?:BsModalRef
  constructor(private propertiesService:PropertiesService, private bsModalService:BsModalService){}
  ngOnInit(): void {
    this.loadProperties();
  }

  loadProperties(){
    this.propertiesService.getProperties().subscribe({
      next:properties=>{
        this.properties = properties;
      },
      error:err=>console.log(err)
    })
  }

  propertyDetails(property:IGetPropery){
    const config = {
      class:'modal-dialog-centered modal-lg',
      initialState:{
        property,
      }
    };

    this.bsModalRef =this.bsModalService.show(PropertyDetailsComponent,config)
  }

}
