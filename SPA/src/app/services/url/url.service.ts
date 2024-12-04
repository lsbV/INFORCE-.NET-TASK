import { Injectable } from '@angular/core';
import {ShortUrlInfo} from '../../models/ShortUrlInfo';
import {EntireUrlInfo} from '../../models/EntireUrlInfo';
import {HttpClient} from '@angular/common/http';
import {UserService} from '../user/user.service';

@Injectable({
  providedIn: 'root'
})
export class UrlService {
  baseUrl = 'http://localhost:8080/api/url';

  constructor(private http: HttpClient,private userService:UserService) { }

  getShortUrls(page: number, count: number) {
    return this.http.get<ShortUrlInfo[]>(`${this.baseUrl}?page=${page}&count=${count}`);
  }

  getShortUrlsForUser(page: number, count: number, userId: number) {
    return this.http.get<ShortUrlInfo[]>(`${this.baseUrl}/user/${userId}?page=${page}&count=${count}`,{
      headers:{
        Authorization: `Bearer ${this.userService.getToken()}`
      }
    });
  }

  createShortUrl(url:string){
    return this.http.post<ShortUrlInfo>(this.baseUrl, {url},{
      headers:{
        Authorization: `Bearer ${this.userService.getToken()}`
      }
    });
  }

  getEntireUrlInfo(shortenedUrl:string) {
    shortenedUrl = shortenedUrl.split('/').pop() || '';
    return this.http.get<EntireUrlInfo>(`${this.baseUrl}/${shortenedUrl}`,{
      headers:{
        Authorization: `Bearer ${this.userService.getToken()}`
      }
    });
  }

  deleteShortUrl(shortenedUrl:string) {
    shortenedUrl = shortenedUrl.split('/').pop() || '';
    return this.http.delete(`${this.baseUrl}/${shortenedUrl}`,{
      headers:{
        Authorization: `Bearer ${this.userService.getToken()}`
      }
    });
  }
}
