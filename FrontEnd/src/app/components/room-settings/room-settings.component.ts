import { Component, OnInit } from '@angular/core';
import { Room } from 'src/app/models/room';
import { RoomService } from 'src/app/services/room/room.service';

@Component({
  selector: 'app-room-settings',
  templateUrl: './room-settings.component.html',
  styleUrls: ['./room-settings.component.css']
})
export class RoomSettingsComponent implements OnInit {

  public roomList: Room[] = [];

  constructor(private roomService: RoomService) { }

  ngOnInit(): void {
    this.getAllRooms();
  }

  getAllRooms() {
    this.roomService.getAllRooms().subscribe(data => {
      console.log("response", data);
      this.roomList = data;
    }, error => {
      console.log("error", error);
    })
  }

}
