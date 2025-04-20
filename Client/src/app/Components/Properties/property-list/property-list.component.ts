import { Component, OnInit } from '@angular/core';
import { PropertiesService } from '../../../Services/properties.service';
import { IGetPropery, Pagination } from '../../../Models/property';
import { CommonModule } from '@angular/common';
import { BsModalRef, BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { PropertyDetailsComponent } from '../property-details/property-details.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { ICategory, IType } from '../../../Models/type';
import { FilterParams } from '../../../Models/filter.params';

@Component({
  selector: 'app-property-list',
  imports: [CommonModule, ModalModule, PaginationModule, FormsModule, ],
  templateUrl: './property-list.component.html',
  styleUrl: './property-list.component.css',
  providers:[
    BsModalService
  ]
})
export class PropertyListComponent implements OnInit {
  properties:IGetPropery[] = []
  types:IType[] = []
  categories:ICategory[]=[]
  pagination:Pagination = new Pagination();
  bsModalRef?:BsModalRef
  filterParams:FilterParams = new FilterParams();
  pageNumber = 1
  pageSize = 6
  constructor(private propertiesService:PropertiesService, private bsModalService:BsModalService){}
  ngOnInit(): void {
    this.loadProperties();
    this.loadTypes();
    this.loadCategories();
  }

  loadProperties(){
    this.propertiesService.getProperties(this.pageNumber, this.pageSize, this.filterParams).subscribe({
      next:res=>{
        // console.log(this.filterParams)
        this.properties = res.data;
        this.pagination = res.pagination
        console.log(res.pagination)
      },
      error:err=>console.log(err)
    })
  }

  loadTypes(){
    this.propertiesService.getTypes().subscribe({
      next:res=>{
        this.types = res
      },
      error:err=>console.log(err)
    })
  }

  loadCategories(){
    this.propertiesService.getCategories().subscribe({
      next:res=>{
        this.categories =res;
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
