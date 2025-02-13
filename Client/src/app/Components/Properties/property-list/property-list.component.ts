import { Component, OnInit } from '@angular/core';
import { PropertiesService } from '../../../Services/properties.service';
import { IGetPropery } from '../../../Models/property';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-property-list',
  imports: [CommonModule],
  templateUrl: './property-list.component.html',
  styleUrl: './property-list.component.css'
})
export class PropertyListComponent implements OnInit {
  properties:IGetPropery[] = []
  constructor(private propertiesService:PropertiesService){}
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

}
