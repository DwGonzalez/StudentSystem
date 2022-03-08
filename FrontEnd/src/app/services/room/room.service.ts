import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { ResponseCode } from 'src/app/enums/responseCode';
import { Constants } from 'src/app/Helper/constants';
import { ResponseModel } from 'src/app/models/responseModel';
import { Room } from 'src/app/models/room';

@Injectable({
  providedIn: 'root'
})
export class RoomService {

  private readonly baseURL: string = "https://localhost:44384/api/room/";

  constructor(private httpClient: HttpClient) { }

  public getAllRooms() {
    let userInfo = JSON.parse(localStorage.getItem(Constants.USER_KEY)!);
    const header = new HttpHeaders({
      'Authorization': `Bearer ${userInfo?.token}`
    })
    return this.httpClient.get<ResponseModel>(this.baseURL + "GetAllRooms", { headers: header })
      .pipe(map(res => {
        let roomList = new Array<Room>();
        if (res.responseCode == ResponseCode.OK) {
          if (res.dataSet) {
            res.dataSet.map((x: Room) => {
              roomList.push(new Room(x.roomName));
            })
          }
        }
        return roomList;
      }));
  }
}
