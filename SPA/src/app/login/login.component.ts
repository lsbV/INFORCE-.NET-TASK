import { Component } from '@angular/core';
import {FormsModule} from '@angular/forms';
import {UserService} from '../services/user/user.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [
    FormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  email: string = '';
  password: string = '';

  constructor(private userService: UserService,private router: Router) {

  }

  login(){
    this.userService.login(this.email, this.password).subscribe((success) => {
      if(success){
        this.router.navigate(['/']).then(r => {
          console.log('Login successful');
          this.email = '';
          this.password = '';
        });

      }else{
        console.log('Login failed');
      }
    });

  }

}
