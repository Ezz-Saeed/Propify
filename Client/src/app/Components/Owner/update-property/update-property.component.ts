import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { IGetPropery } from '../../../Models/property';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TextInputComponent } from '../../text-input/text-input.component';
import { PropertiesService } from '../../../Services/properties.service';
import { IType } from '../../../Models/type';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryModule, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { IPhoto } from '../../../Models/photo';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { DeletePropertyImageComponent } from '../delete-property-image/delete-property-image.component';

@Component({
  selector: 'app-update-property',
  imports: [CommonModule,FormsModule, ReactiveFormsModule, TextInputComponent, NgxGalleryModule],
  templateUrl: './update-property.component.html',
  styleUrl: './update-property.component.css',
  providers:[
    BsModalService
  ]
})
export class UpdatePropertyComponent implements OnInit {
  @Input() updatedProperty = new EventEmitter();
  property!:IGetPropery
  propertyForm!:FormGroup
  propertyTypes: IType[] = []
  options!:NgxGalleryOptions[]
  galleryImages!:NgxGalleryImage[]
  propertyImages!:IPhoto[]
  selectedFile: File | null = null;
  previewImage: string | undefined = undefined;
  deleteImageModalRef!:BsModalRef;

  constructor(public bsModalref:BsModalRef,private fb:FormBuilder,
    private propertiesService:PropertiesService, private bsModalService:BsModalService){}
  ngOnInit(): void {
    this.options = [
          {
            width:'300px',
            height:'300px',
            imagePercent:100,
            // thumbnailsColumns:4,
            thumbnails:false,
            imageAnimation:NgxGalleryAnimation.Slide,
            preview:false
          }
        ]
    this.inintializeForm();
    this.galleryImages = this.getPropertyImages();
    this.propertyImages = this.property.images;
    this.loadTypes();
  }

  private inintializeForm(){
    this.propertyForm = this.fb.group({
      description: [this.property.description, Validators.required],
      address: [this.property.address, Validators.required],
      city: [this.property.city, Validators.required],
      price: [this.property.price, Validators.required],
      bedRooms: [this.property.bedRooms, ],
      bathRooms: [this.property.bathRooms,],
      area: [this.property.area, Validators.required],
      isRental: [this.property.isRental,Validators.required],
      isAvailable: [this.property.isAvailable,Validators.required],
      typeId: [this.property.typeId, Validators.required],
      // images: ['',]
    })
  }

  private getPropertyImages():NgxGalleryImage[]{
    const images = [];
    for(let image of this.property.images){
        images.push({
          small:image.url,
          medium:image.url,
          big:image.url,
          isMain: image.isMain,
          propertyId:image.propertyId,
          publicId:image.publicId
        })
    }
    return images;
  }

  uploadImage(){
    if(this.selectedFile){
      this.propertiesService.uploadImage(this.property.id, this.selectedFile).subscribe({
        next:res=>{
          const image = new IPhoto()
          image.url = res.url
          image.isMain = res.isMain
          this.property.images.push(image);
          this.previewImage = undefined;
          this.bsModalref.hide();
        },
        error:err=>console.log(err)
      })
    }
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      const reader = new FileReader();
      reader.onload = (e) => {
        this.previewImage = e.target?.result as string;
      };
      reader.readAsDataURL(this.selectedFile);
    } else {
      this.previewImage = undefined;
    }
  }

  deleteImage(propertyId:number, publicId:string){
    const config = {
      class: "modal-dialog-centered",
      initialState:{
        propertyId,
        publicId
      }
    }
    this.deleteImageModalRef = this.bsModalService.show(DeletePropertyImageComponent, config);
    this.deleteImageModalRef.content.deletedImagePublicId.subscribe({
      next:(publicId:string)=>{
        const imgIndex = this.property.images.findIndex(img=>img.publicId == publicId)
        this.property.images.splice(imgIndex,1)
      }
    })
  }

  loadTypes(){
    this.propertiesService.getTypes().subscribe({
      next: res=>this.propertyTypes = res,
      error:err=>console.log(err)
    })
  }

  saveProperty(){
    if(this.propertyForm.valid){
      this.propertiesService.updateProperty(this.propertyForm.value, this.property.id).subscribe({
        next:res=>{
          this.property = res;
          this.updatedProperty.emit(this.property)
          this.bsModalref.hide();
        },
        error:err=>console.log(err),
      })
    }
  }


  close(){
    this.bsModalref.hide();
  }
}
