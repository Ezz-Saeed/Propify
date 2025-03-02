import { Injectable } from '@angular/core';
import { Environment } from '../Environments/environment';
import { HttpClient } from '@angular/common/http';
import { IGetPropery } from '../Models/property';
import { IType } from '../Models/type';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class PropertiesService {
  baseUrl = `${Environment.API_URL}/Properties`

  constructor(private http:HttpClient) { }

  getProperties(){
    return this.http.get<IGetPropery[]>(`${this.baseUrl}/properties`);
  }

  addProperty(propertyForm: FormGroup) {
    const formData = new FormData();

    // Append all form values
    Object.keys(propertyForm.value).forEach((key) => {
      formData.append(key, propertyForm.value[key]);
    });

    // Append images
    if (propertyForm.value.images && propertyForm.value.images.length > 0) {
      for (let file of propertyForm.value.images) {
        formData.append('images', file);
      }
    }

    return this.http.post<IGetPropery>(`${this.baseUrl}/addProperty`, formData);
  }


  getTypes(){
    return this.http.get<IType[]>(`${this.baseUrl}/types`)
  }
}
