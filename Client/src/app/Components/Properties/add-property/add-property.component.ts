import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TextInputComponent } from "../../text-input/text-input.component";
import { CommonModule } from '@angular/common';
import { IType } from '../../../Models/type';
import { PropertiesService } from '../../../Services/properties.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-property',
  imports: [ReactiveFormsModule, TextInputComponent, FormsModule, CommonModule],
  templateUrl: './add-property.component.html',
  styleUrl: './add-property.component.css'
})
export class AddPropertyComponent implements OnInit {
  propertyForm!:FormGroup
  previewImages: string[] = [];
  propertyTypes: IType[] = []
  selectedFiles: File[] = [];

  constructor(private fb:FormBuilder, private propertiesService:PropertiesService,
    private router:Router){}
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
    if (event.target.files) {
      for (let file of event.target.files) {
        this.selectedFiles.push(file);
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.previewImages.push(e.target.result);
        };
        reader.readAsDataURL(file);
      }
    }
  }

  removeImage(index: number) {
    this.previewImages.splice(index, 1);
    this.propertyForm.get('images')?.setValue(this.previewImages);
  }

  onSubmit() {
    if (this.propertyForm.valid) {
      this.propertiesService.addProperty(this.propertyForm, this.selectedFiles).subscribe({
        next: res=>{
          this.router.navigate(['/ownerDashboard',{tab:1}])
          // console.log(res)
        },
        error:err=>console.log(err)
      })
    }
  }

}
