import { Injectable } from '@angular/core';
import { catchError, map, ReplaySubject, throwError } from 'rxjs';
import { IAuthUser } from '../Models/auth.user';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Environment } from '../Environments/environment';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = `${Environment.API_URL}/Account`;
  currentUserSource = new ReplaySubject<IAuthUser | null>(1);
  currentUser$ = this.currentUserSource.asObservable();
  refreshToken?:string;
  errorMessage: string | null = null;
  constructor(private http:HttpClient, ) { }

  logIn(model: any) {
    return this.http.post<IAuthUser>(`${this.baseUrl}/getToken`, model, { withCredentials: true }).pipe(
      map((res: IAuthUser) => {
        this.loadCurrentUser(res);
        this.errorMessage = res.message ?? null
        return res;
      }),
    );
}


  register(model:any){
    return this.http.post<IAuthUser>(`${this.baseUrl}/register`, model,
      {withCredentials:true}).pipe(
      map(user=>{
        if(user){
          this.loadCurrentUser(user);
        }
        this.errorMessage = user.message ?? null
        return user;
      }),
      // catchError(this.handleError.bind(this))
    )
  }

  getNewToken(){
    return this.http.get<IAuthUser>(`${this.baseUrl}/refreshToken`,{withCredentials:true}).pipe(
      map(user=>{
        if(user){
          console.log(user)
          this.loadCurrentUser(user)
        }
        return user
      })
    )
  }

  loadCurrentUser(user:IAuthUser){
    this.currentUserSource.next(user)
    localStorage.setItem('token', user.token);
  }

  loadCurrentUserUsingToken(token:string | null){
    if(token === null){
      this.currentUserSource.next(null)
    }
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`)
    return this.http.get<IAuthUser>(`${this.baseUrl}/loadCurrentUser`,{headers:headers}).pipe(
      map((user:IAuthUser)=>{
        if(user){
          this.loadCurrentUser(user)
        }
        return user;
      })
    )
  }

  editProfile(data:FormGroup, image?:File){
    const formData = new FormData();
    Object.keys(data.value).forEach((key)=>{
      formData.append(key, data.value[key])
    })
    // console.log(formData)
    if(image){
      formData.append('profileImage', image)
    }
    return this.http.put(`${this.baseUrl}/editProfile/`, formData)
  }

}
