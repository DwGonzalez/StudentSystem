import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { ResponseCode } from 'src/app/enums/responseCode';
import { Constants } from 'src/app/Helper/constants';
import { ResponseModel } from 'src/app/models/responseModel';
import { User } from 'src/app/models/user';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  private readonly baseURL: string = "https://localhost:44384/api/student/";

  constructor(private httpClient: HttpClient) { }

  public getMySubjects() {
    let userInfo = JSON.parse(localStorage.getItem(Constants.USER_KEY)!);
    const header = new HttpHeaders({
      'Authorization': `Bearer ${userInfo?.token}`
    })
    return this.httpClient.get<ResponseModel>(this.baseURL + "GetStudentSubjects", { headers: header })
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
