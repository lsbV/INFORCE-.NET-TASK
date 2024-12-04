import {Component, EventEmitter, Input, Output} from '@angular/core';
import {ShortUrlInfo} from '../models/ShortUrlInfo';
import {Router} from '@angular/router';
import {UrlService} from '../services/url/url.service';

@Component({
  selector: 'app-urls-table',
  imports: [],
  templateUrl: './urls-table.component.html',
  styleUrl: './urls-table.component.css'
})
export class UrlsTableComponent {
  @Input({required:true}) urls: ShortUrlInfo[] = [];
  @Input({required:true}) showActionButtons = false;
  @Output() reloadUrls = new EventEmitter<void>();


  constructor(private router: Router, private urlService: UrlService) {
  }

  reload(){
    this.reloadUrls.emit();
  }


  showDetails(shortenedUrl: string) {
    this.router.navigate(['/info', shortenedUrl]).then(console.log).catch(console.error);
  }

  deleteUrl(shortenedUrl: string) {
    this.urlService.deleteShortUrl(shortenedUrl).subscribe(() => {
      this.reload();
    });

  }
}
