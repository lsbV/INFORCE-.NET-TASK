import { Component } from '@angular/core';
import {ShortUrlInfo} from '../models/ShortUrlInfo';
import {UrlService} from '../services/url/url.service';
import {UrlsTableComponent} from '../urls-table/urls-table.component';
import {UserService} from '../services/user/user.service';

@Component({
  selector: 'app-home',
  imports: [
    UrlsTableComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  urls:ShortUrlInfo[] = [];
  page = 1;
  pageSize = 10;


  constructor(private urlService: UrlService,public userService: UserService) {
    this.urlService.getShortUrls(this.page, this.pageSize).subscribe((data) => {
      this.urls = data;
    });

  }

  getUrls(){
    this.urlService.getShortUrls(this.page, this.pageSize).subscribe((data) => {
      this.urls = data;
    });
  }
}
