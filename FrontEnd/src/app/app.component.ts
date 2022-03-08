import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Constants } from './Helper/constants';
import { User } from './models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'StudentSystem';

  constructor(private router: Router) { }

  onLogout() {
    localStorage.removeItem(Constants.USER_KEY);
    this.router.navigate(['/login']);
  }

  get isUserLogged() {
    const user = localStorage.getItem(Constants.USER_KEY);
    //console.log(user);
    return user && user.length > 0;
  }

  get user(): User {
    return JSON.parse(localStorage.getItem(Constants.USER_KEY)!) as User;
  }

  get isAdmin(): boolean {
    return this.user.role.indexOf('Admin') > -1;
  }

}
