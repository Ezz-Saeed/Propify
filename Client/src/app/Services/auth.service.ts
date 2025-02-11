import { Injectable } from '@angular/core';
import { map, ReplaySubject } from 'rxjs';
import { IAuthUser } from '../Models/auth.user';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = 'http://localhost:5269/api/Account';
  currentUserSource = new ReplaySubject<IAuthUser | null>(1);
  currentUser$ = this.currentUserSource.asObservable();
  refreshToken?:string;
  constructor(private http:HttpClient, ) { }

  logIn(model:any){
    return this.http.post<IAuthUser>(`${this.baseUrl}/getToken`,model,
      {withCredentials:true}).pipe(
      map((res:IAuthUser)=>{
        this.loadCurrentUser(res)
        return res;
      })
    )
  }

  register(model:any){
    return this.http.post<IAuthUser>(`${this.baseUrl}/register`, model,
      {withCredentials:true}).pipe(
      map(user=>{
        if(user){
          this.loadCurrentUser(user);
        }
        return user;
      })
    )
  }

  getNewToken(){
    return this.http.get<IAuthUser>(`${this.baseUrl}/refreshToken`,{withCredentials:true}).pipe(
      map(user=>{
        if(user){
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
          this.currentUserSource.next(user)
        }
        return user;
      })
    )
  }
}
