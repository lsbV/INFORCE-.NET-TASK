import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {UrlService} from '../services/url/url.service';
import {EntireUrlInfo} from '../models/EntireUrlInfo';
import {DatePipe} from '@angular/common';

@Component({
  selector: 'app-url-info',
  templateUrl: './url-info.component.html',
  imports: [
    DatePipe
  ],
  styleUrl: './url-info.component.css'
})
export class UrlInfoComponent implements OnInit {
  id: string = '';
  entireUrlInfo: EntireUrlInfo|undefined;

  constructor(private route: ActivatedRoute, private urlService: UrlService) {
  }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id')!;
    this.urlService.getEntireUrlInfo(this.id).subscribe((data) => {
      this.entireUrlInfo = data;
    });
  }

  deleteUrl(){
    this.urlService.deleteShortUrl(this.id).subscribe(() => {
      console.log('Deleted');
    });
  }
}
