import { Component, OnInit } from '@angular/core';
import { PropertiesService } from '../../../Services/properties.service';
import { IGetPropery, Pagination } from '../../../Models/property';
import { CommonModule } from '@angular/common';
import { BsModalRef, BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { PropertyDetailsComponent } from '../property-details/property-details.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-property-list',
  imports: [CommonModule, ModalModule, PaginationModule, FormsModule],
  templateUrl: './property-list.component.html',
  styleUrl: './property-list.component.css',
  providers:[
    BsModalService
  ]
})
export class PropertyListComponent implements OnInit {
  properties:IGetPropery[] = []
  pagination!:Pagination
  bsModalRef?:BsModalRef
  pageNumber = 1
  pageSize = 6
  constructor(private propertiesService:PropertiesService, private bsModalService:BsModalService){}
  ngOnInit(): void {
    this.loadProperties();
  }

  loadProperties(){
    this.propertiesService.getProperties(this.pageNumber, this.pageSize).subscribe({
      next:res=>{
        // console.log(res)
        this.properties = res.data;
        this.pagination = res.pagination
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

  pageChanged(event:any){
    this.pageNumber = event.page;
    this.loadProperties();
  }

}
