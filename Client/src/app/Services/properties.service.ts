import { Injectable } from '@angular/core';
import { Environment } from '../Environments/environment';
import { HttpClient } from '@angular/common/http';
import { IGetPropery } from '../Models/property';
import { IType } from '../Models/type';
import { FormGroup } from '@angular/forms';
import { IPhoto } from '../Models/photo';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PropertiesService {
  baseUrl = `${Environment.API_URL}/Properties`
  properties:IGetPropery[] = []

  constructor(private http:HttpClient) { }

  getProperties(){
    return this.http.get<IGetPropery[]>(`${this.baseUrl}/properties`);
  }

  getPropertiesForOwner(){
    if(this.properties.length > 0){
      // console.log(this.properties)
      return of(this.properties)
    }
    return this.http.get<IGetPropery[]>(`${this.baseUrl}/ownerProperties`).pipe(
      map((res:IGetPropery[])=>{
        this.properties = res
        return res
      })
    );
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

    return this.http.post<IGetPropery>(`${this.baseUrl}/addProperty`, formData).pipe(
      map((p:IGetPropery)=>{
          this.properties.push(p)
          // console.log(p)
          return p
      })
    );
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
    return this.http.put(`${this.baseUrl}/deleteProperty/${id}`,{})
  }

  deleteImage(id:number, publicId:string){
    return this.http.delete(`${this.baseUrl}/deleteImage/${id}?publicId=${publicId}`)
  }
}
