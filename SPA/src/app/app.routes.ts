import { Routes } from '@angular/router';
import {HomeComponent} from './home/home.component';
import {AboutComponent} from './about/about.component';
import {LoginComponent} from './login/login.component';
import {AddNewUrlComponent} from './add-new-url/add-new-url.component';
import {UrlInfoComponent} from './url-info/url-info.component';
import {MyUrlsComponent} from './my-urls/my-urls.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'about', component: AboutComponent },
  { path: 'login', component: LoginComponent },
  { path: 'new', component: AddNewUrlComponent },
  { path: 'info/:id', component: UrlInfoComponent },
  { path: 'my-urls', component: MyUrlsComponent },
  { path: '**', redirectTo: '/home' }
];
