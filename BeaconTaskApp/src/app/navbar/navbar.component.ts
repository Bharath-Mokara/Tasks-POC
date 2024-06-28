import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit{

  constructor(public authService: AuthService, private router:Router){}

  ngOnInit(): void {
    this.checkLoginStatus();
  }

  checkLoginStatus(): void{
    this.authService.isLoggedIn = localStorage.getItem("token") !== null;
  }

  performLogout() : void
  {
    //clearing the token
    this.authService.logout().subscribe({
      next:(response) =>{
        localStorage.clear();
        this.authService.isLoggedIn = false;
        this.router.navigate(['/']);
      },
      error: (error)=>{
        console.log(error);
      }
    })
  }
}
