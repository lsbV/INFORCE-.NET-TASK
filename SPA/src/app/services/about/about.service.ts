import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {UserService} from '../user/user.service';

@Injectable({
  providedIn: 'root'
})
export class AboutService {

  private baseUrl = 'http://localhost:8080/api/about';
  constructor(private http:HttpClient,private userService:UserService)
  {
  }

  getAbout()
  {
    return this.http.get<{ text:string }>(this.baseUrl);
  }

  updateAbout(about:string)
  {
    return this.http.put<{ message:string }>(this.baseUrl, {text:about}, {
      headers:{
        Authorization: `Bearer ${this.userService.getToken()}`
      }
    });
  }
}
