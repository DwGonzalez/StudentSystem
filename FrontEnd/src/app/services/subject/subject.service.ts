import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Subject } from 'rxjs';
import { ResponseCode } from 'src/app/enums/responseCode';
import { Constants } from 'src/app/Helper/constants';
import { ResponseModel } from 'src/app/models/responseModel';
import { Room } from 'src/app/models/room';
import { SubjectClass } from 'src/app/models/subject';
import { User } from 'src/app/models/user';

@Injectable({
    providedIn: 'root'
})
export class SubjectService {

    private readonly baseURL: string = "https://localhost:44384/api/subject/";

    constructor(private httpClient: HttpClient) { }

    public getMySubjects() {
        let userInfo = JSON.parse(localStorage.getItem(Constants.USER_KEY)!);
        let email = userInfo.email;
        const header = new HttpHeaders({
            'Authorization': `Bearer ${userInfo?.token}`
        })
        return this.httpClient.get<ResponseModel>(this.baseURL + `GetStudentSubjects/?email=${email}`, { headers: header })
            .pipe(map(res => {
                let subjectList = new Array<SubjectClass>();
                if (res.responseCode == ResponseCode.OK) {
                    if (res.dataSet) {
                        //console.log(res.dataSet);
                        res.dataSet.map((x: any) => {
                            //console.log(x)
                            subjectList.push(new SubjectClass(x.subject.subjectId, x.subject.subjectName, x.subject.professor, x.student, x.subject.room));
                        })
                    }
                }
                return subjectList;
            }));
    }
}
