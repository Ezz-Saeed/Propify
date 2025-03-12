import { Component, OnInit } from '@angular/core';
import { IGetPropery } from '../../../Models/property';
import { PropertiesService } from '../../../Services/properties.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-properties',
  imports: [CommonModule],
  templateUrl: './properties.component.html',
  styleUrl: './properties.component.css'
})
export class PropertiesComponent implements OnInit {
  properties!:IGetPropery[];
  constructor(private propertyService:PropertiesService){}
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
}
