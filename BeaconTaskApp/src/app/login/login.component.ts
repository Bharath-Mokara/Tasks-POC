import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { LoginViewModel } from '../models/login-view-model';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm !: FormGroup;

  constructor(public authService: AuthService,private router: Router){}

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username: new FormControl(null),
      password: new FormControl(null)
    });
  }


  onLoginSubmit()
  {
    if(this.loginForm.valid)
    {
      var loginViewModel = this.loginForm.value as LoginViewModel;
      this.authService.Login(loginViewModel).subscribe({
        next :(response) => {
          this.loginForm.reset();
          this.authService.isLoggedIn = true;
          localStorage.setItem("token",response.result["token"]);
          localStorage.setItem("userId",response.result["user"].id)
          this.router.navigate(['/resource']);
        },
        error: (error) =>{
          console.log(error);
        }
     }); 

    }
  }


}
