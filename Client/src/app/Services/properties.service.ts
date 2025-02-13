import { Injectable } from '@angular/core';
import { Environment } from '../Environments/environment';
import { HttpClient } from '@angular/common/http';
import { IGetPropery } from '../Models/property';

@Injectable({
  providedIn: 'root'
})
export class PropertiesService {
  baseUrl = `${Environment.API_URL}/Properties`

  constructor(private http:HttpClient) { }

  getProperties(){
    return this.http.get<IGetPropery[]>(`${this.baseUrl}/properties`);
  }
}
