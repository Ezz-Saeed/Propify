import { Injectable } from '@angular/core';
import { Environment } from '../Environments/environment';
import { HttpClient } from '@angular/common/http';
import { IGetPropery } from '../Models/property';
import { IType } from '../Models/type';
import { FormGroup } from '@angular/forms';
import { IPhoto } from '../Models/photo';

@Injectable({
  providedIn: 'root'
})
export class PropertiesService {
  baseUrl = `${Environment.API_URL}/Properties`

  constructor(private http:HttpClient) { }

  getProperties(){
    return this.http.get<IGetPropery[]>(`${this.baseUrl}/properties`);
  }

  getPropertiesForOwner(){
    return this.http.get<IGetPropery[]>(`${this.baseUrl}/ownerProperties`);
  }

  addProperty(propertyForm: FormGroup, images:File[]) {
    const formData = new FormData();

    Object.keys(propertyForm.value).forEach((key) => {
      formData.append(key, propertyForm.value[key]);
    });

    if(images && images.length > 0){
      images.forEach((file, index) => {
        formData.append('images', file);
      });
    }

    return this.http.post<IGetPropery>(`${this.baseUrl}/addProperty`, formData);
  }

  updateProperty(model:any, id:number){
    return this.http.put<IGetPropery>(`${this.baseUrl}/updateProperty/${id}`, model)
  }


  getTypes(){
    return this.http.get<IType[]>(`${this.baseUrl}/types`)
  }

  uploadImage(id:number, file:File){
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<IPhoto>(`${this.baseUrl}/uploadImage/${id}`,formData);
  }

  deleteProperty(id:number){
    return this.http.delete(`${this.baseUrl}/deleteImage/${id}`)
  }
}
