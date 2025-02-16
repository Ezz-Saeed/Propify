import { Component, OnInit } from '@angular/core';
import { IGetPropery } from '../../../Models/property';
import { CommonModule } from '@angular/common';
import { BsModalRef } from 'ngx-bootstrap/modal';
import {NgxGalleryAnimation, NgxGalleryImage, NgxGalleryModule, NgxGalleryOptions} from '@kolkov/ngx-gallery'

@Component({
  selector: 'app-property-details',
  imports: [CommonModule, NgxGalleryModule],
  templateUrl: './property-details.component.html',
  styleUrl: './property-details.component.css'
})
export class PropertyDetailsComponent implements OnInit {
  property!:IGetPropery
  galleryOptions!:NgxGalleryOptions[];
  galleryImages!:NgxGalleryImage[]
  constructor(public bsModalRef:BsModalRef){}
  ngOnInit(): void {
    this.galleryOptions = [
      {
        width:'500px',
        height:'500px',
        imagePercent:100,
        thumbnailsColumns:4,
        imageAnimation:NgxGalleryAnimation.Slide,
        preview:false
      }
    ]
    this.galleryImages = this.getPropertyImages();
  }

  getPropertyImages():NgxGalleryImage[]{
    const images = [];
    for(let image of this.property.images){
      images.push({
        small:image.url,
        medium:image.url,
        big:image.url,
      })
    }
    return images;
  }

  close(){
    this.bsModalRef.hide();
  }
}
