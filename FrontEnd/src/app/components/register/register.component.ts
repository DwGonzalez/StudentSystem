import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  public registerForm = this.formBuilder.group({
    fullName: ['', Validators.required],
    email: ['', [Validators.email, Validators.required]],
    password: ['', Validators.required]
  })

  constructor(private formBuilder: FormBuilder, private userService: UserService) { }

  ngOnInit(): void {
  }

  onSubmit() {
    console.log('registered');
    let fullName = this.registerForm.controls["fullName"].value;
    let email = this.registerForm.controls["email"].value;
    let password = this.registerForm.controls["password"].value;
    this.userService.register(fullName, email, password).subscribe((data) => {
      this.registerForm.controls["fullName"].setValue("");
      this.registerForm.controls["email"].setValue("");
      this.registerForm.controls["password"].setValue("");
      //console.log("response", data);
    }, error => {
      console.log("Error", error);
    })
  }

}
