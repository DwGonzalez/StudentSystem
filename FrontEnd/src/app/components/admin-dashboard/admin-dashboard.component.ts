import { Component, OnInit } from '@angular/core';
import { Constants } from 'src/app/Helper/constants';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  get user(): User {
    return JSON.parse(localStorage.getItem(Constants.USER_KEY)!) as User;
  }

  get isAdmin(): boolean {
    return this.user.role.indexOf('Admin') > -1;
  }

}
