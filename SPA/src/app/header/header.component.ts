import { Component } from '@angular/core';
import {RouterLink} from '@angular/router';
import {UserService} from '../services/user/user.service';

@Component({
  selector: 'app-header',
  imports: [
    RouterLink
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {

  constructor(public userService: UserService) {

  }

  logout(){
    this.userService.logout();
  }

}
