import { Component } from '@angular/core';
import {UrlService} from '../services/url/url.service';
import {UrlsTableComponent} from '../urls-table/urls-table.component';
import {UserService} from '../services/user/user.service';
import {ShortUrlInfo} from '../models/ShortUrlInfo';

@Component({
  selector: 'app-my-urls',
  imports: [
    UrlsTableComponent
  ],
  templateUrl: './my-urls.component.html',
  styleUrl: './my-urls.component.css'
})
export class MyUrlsComponent {
  public urls:ShortUrlInfo[] = [];
  public page = 1;
  public pageSize = 10;

  constructor(private urlService:UrlService, private userService:UserService) {
    this.getUrls();
  }

  getUrls() {
    if ( !this.userService.isLoggedInUser() ) {
      return;
    }
    this.urlService.getShortUrlsForUser(this.page, this.pageSize, this.userService.getId()!).subscribe((data) => {
      this.urls = data;
    });
  }
}
