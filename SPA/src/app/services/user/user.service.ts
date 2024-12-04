import { Injectable } from '@angular/core';
import {LoginResponse} from '../../models/LoginResponse';
import {HttpClient} from '@angular/common/http';
import {catchError, map, of} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = 'http://localhost:8080/api/user';
  private isLoggedIn = false;
  private userInfo:LoginResponse|null = null;

  constructor(private http: HttpClient) {
    const userInfoStr = localStorage.getItem('userInfo');
    if(userInfoStr) {
      this.userInfo = JSON.parse(userInfoStr);
      this.isLoggedIn = true;
    }
  }

  public login(email:string, password:string) {
    if (this.isLoggedIn) {
      return of(true);
    }
    return this.http.post<LoginResponse>(`${this.baseUrl}/login`, { email, password }).pipe(
      map(response => {
        this.isLoggedIn = true;
        this.userInfo = response;

        localStorage.setItem('userInfo', JSON.stringify(this.userInfo));

        return true;
      }),
      catchError(error => {
        console.error('Login failed', error);
        return of(false);
      })
    );
  }

  public logout(){
    this.isLoggedIn = false;
    this.userInfo = null;
    localStorage.removeItem('userInfo');
  }

  public isLoggedInUser():boolean{
    return this.isLoggedIn;
  }

  public getEmail():string|undefined{
    return this.userInfo?.email;
  }

  public getRole():string|undefined{
    return this.userInfo?.role;
  }

  public isAdmin():boolean{
    return this.userInfo?.role === 'Admin';
  }

  public getToken():string|undefined{
    return this.userInfo?.token;
  }

  getId() {
    return this.userInfo?.id;
  }
}
