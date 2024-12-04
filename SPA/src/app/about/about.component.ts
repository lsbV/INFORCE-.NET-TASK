import { Component } from '@angular/core';
import {AboutService} from '../services/about/about.service';
import {FormsModule} from '@angular/forms';
import {NgStyle} from '@angular/common';
import {UserService} from '../services/user/user.service';

@Component({
  selector: 'app-about',
  imports: [
    FormsModule,
    NgStyle
  ],
  templateUrl: './about.component.html',
  styleUrl: './about.component.css'
})
export class AboutComponent {
  about: string = "";
  backup: string = "";
  editMode: boolean = false;

  constructor(private aboutService: AboutService, public userService: UserService) {
    this.aboutService.getAbout().subscribe((data) => {
      this.about = data.text;
      this.backup = data.text;
    });
  }

  save(){
    this.editMode = false;

    this.aboutService.updateAbout(this.about).subscribe((data) => {
      alert('About updated');
    });
  }

  edit(){
    this.backup = this.about;
    this.editMode = true;
  }

  cancel(){
    this.editMode = false;
    this.about = this.backup;
  }

}
