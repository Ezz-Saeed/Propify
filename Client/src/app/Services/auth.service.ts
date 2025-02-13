import { Injectable } from '@angular/core';
import { catchError, map, ReplaySubject, throwError } from 'rxjs';
import { IAuthUser } from '../Models/auth.user';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = 'http://localhost:5269/api/Account';
  currentUserSource = new ReplaySubject<IAuthUser | null>(1);
  currentUser$ = this.currentUserSource.asObservable();
  refreshToken?:string;
  errorMessage: string | null = null;
  constructor(private http:HttpClient, ) { }

  logIn(model:any){
    return this.http.post<IAuthUser>(`${this.baseUrl}/getToken`,model,
      {withCredentials:true}).pipe(
      map((res:IAuthUser)=>{
        this.loadCurrentUser(res)
        this.errorMessage = null
        return res;
      }),
      catchError(this.handleError.bind(this))
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
      }),
      catchError(this.handleError.bind(this))
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

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      this.errorMessage = 'A network error occurred. Please try again.';
    } else {
      // Server-side error
      this.errorMessage = error.error.message || 'Error! Check your inputs.';
    }
    return throwError(() => new Error(this.errorMessage!));
  }
}
