import { Component } from '@angular/core';
import {FormsModule} from '@angular/forms';
import {UrlService} from '../services/url/url.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-add-new-url',
  imports: [
    FormsModule
  ],
  templateUrl: './add-new-url.component.html',
  styleUrl: './add-new-url.component.css'
})
export class AddNewUrlComponent {
  url: string = '';

  constructor( private urlService: UrlService, private router: Router) {
  }

  shorten() {
    this.urlService.createShortUrl(this.url).subscribe((data) => {
      this.router.navigate(['/']);
    });
  }
}
