import { Component, OnInit } from '@angular/core';
import { ResponseModel } from 'src/app/models/responseModel';
import { SubjectClass } from 'src/app/models/subject';
import { SubjectService } from 'src/app/services/subject/subject.service';

@Component({
  selector: 'app-subject-settings',
  templateUrl: './subject-settings.component.html',
  styleUrls: ['./subject-settings.component.css']
})
export class SubjectSettingsComponent implements OnInit {

  public subjectClassList: SubjectClass[] = [];

  constructor(private subjectService: SubjectService) { }

  ngOnInit(): void {
    this.getSubjects();
  }

  getSubjects() {
    this.subjectService.getMySubjects().subscribe((data) => {
      console.log("response", data);
      this.subjectClassList = data;
    }, error => {
      console.log("error", error);
    })
  }

}
