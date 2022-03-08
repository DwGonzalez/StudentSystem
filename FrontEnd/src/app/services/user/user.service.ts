import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { ResponseCode } from '../../enums/responseCode';
import { Constants } from '../../Helper/constants';
import { ResponseModel } from '../../models/responseModel';
import { User } from '../../models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly baseURL: string = "https://localhost:44384/api/user/";

  constructor(private httpClient: HttpClient) { }

  public login(email: string, password: string) {
    const body = {
      email: email,
      password: password
    }
    return this.httpClient.post<ResponseModel>(this.baseURL + "Login", body);
  }

  public register(fullName: string, email: string, password: string) {
    const body = {
      FullName: fullName,
      Email: email,
      Password: password
    }
    return this.httpClient.post<ResponseModel>(this.baseURL + "RegisterUser", body);
  }

  public getAllUsers() {
    let userInfo = JSON.parse(localStorage.getItem(Constants.USER_KEY)!);
    const header = new HttpHeaders({
      'Authorization': `Bearer ${userInfo?.token}`
    })
    return this.httpClient.get<ResponseModel>(this.baseURL + "GetAllUsers", { headers: header })
      .pipe(map(res => {
        let userList = new Array<User>();
        if (res.responseCode == ResponseCode.OK) {
          if (res.dataSet) {
            res.dataSet.map((x: User) => {
              userList.push(new User(x.fullName, x.email, x.userName, x.role));
            })
          }
        }
        return userList;
      }));
  }

  public getAllRoles() {
    let userInfo = JSON.parse(localStorage.getItem(Constants.USER_KEY)!);
    const header = new HttpHeaders({
      'Authorization': `Bearer ${userInfo?.token}`
    })
    return this.httpClient.get<ResponseModel>(this.baseURL + "GetAllUsers", { headers: header })
      .pipe(map(res => {
        let userList = new Array<User>();
        if (res.responseCode == ResponseCode.OK) {
          if (res.dataSet) {
            res.dataSet.map((x: User) => {
              userList.push(new User(x.fullName, x.email, x.userName, x.role));
            })
          }
        }
        return userList;
      }));
  }
}
