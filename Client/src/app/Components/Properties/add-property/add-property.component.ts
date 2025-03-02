import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TextInputComponent } from "../../text-input/text-input.component";
import { CommonModule } from '@angular/common';
import { IType } from '../../../Models/type';
import { PropertiesService } from '../../../Services/properties.service';

@Component({
  selector: 'app-add-property',
  imports: [ReactiveFormsModule, TextInputComponent, FormsModule, CommonModule],
  templateUrl: './add-property.component.html',
  styleUrl: './add-property.component.css'
})
export class AddPropertyComponent implements OnInit {
  propertyForm!:FormGroup
  selectedImages: string[] = [];
  propertyTypes: IType[] = []

  constructor(private fb:FormBuilder, private propertiesService:PropertiesService){}
  ngOnInit(): void {
    this.inintializeForm();
    this.loadTypes();
  }


  private inintializeForm(){
    this.propertyForm = this.fb.group({
      description: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      price: ['', Validators.required],
      bedRooms: ['', ],
      bathRooms: ['',],
      area: ['', Validators.required],
      isRental: ['', Validators.required],
      typeId: ['', Validators.required],
      images: ['',]
    })
  }

  loadTypes(){
    this.propertiesService.getTypes().subscribe({
      next: res=>this.propertyTypes = res,
      error:err=>console.log(err)
    })
  }

  onFileSelected(event: any) {
    if (event.target.files.length > 0) {
      for (let file of event.target.files) {
        const reader = new FileReader();
        reader.onload = () => {
          this.selectedImages.push(reader.result as string);
          this.propertyForm.get('images')?.setValue(this.selectedImages);
        };
        reader.readAsDataURL(file);
      }
    }
  }

  removeImage(index: number) {
    this.selectedImages.splice(index, 1);
    this.propertyForm.get('images')?.setValue(this.selectedImages);
  }

  onSubmit() {
    if (this.propertyForm.valid) {
      this.propertiesService.addProperty(this.propertyForm).subscribe({
        next: res=>{

        },
        error:err=>console.log(err)
      })
    }
  }

}
