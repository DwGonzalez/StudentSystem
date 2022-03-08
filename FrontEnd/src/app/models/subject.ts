import { Room } from "./room";
import { User } from "./user";

export class SubjectClass {
    public subjectId: number;
    public subjectName: string = "";
    public professor: User;
    public user: User;
    public room: Room;

    constructor(subjectId: number, subjectName: string, professor: User, user: User, room: Room) {
        this.subjectId = subjectId,
            this.subjectName = subjectName,
            this.professor = professor,
            this.user = user,
            this.room = room
    }
}