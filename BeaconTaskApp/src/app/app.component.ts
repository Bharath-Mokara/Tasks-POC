import { Component, HostListener, OnInit } from '@angular/core';
import { LockService } from './services/lock-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  constructor() {

  }

  ngOnInit(): void {
    this.checkLoginStatus();
  }

  checkLoginStatus(): boolean{
    return localStorage.getItem("token") !== null;
  }

  ngOnDestroy() {
  
  }
}


