import { JsonPipe } from '@angular/common';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LockService {

  apiUrl !: string;

  constructor() { 
    this.apiUrl = environment.apiBaseAddress + "/api/Auth/release-access";
  }

  sendUserExitData() {

    // // Use the Beacon API to send the data
    // const blob = new Blob([JSON.stringify(data)], { type: 'application/json' });
    // navigator.sendBeacon(this.apiUrl,blob);

    var data = {
      id: localStorage.getItem("userId"),
      message: "User lock released",
      lockStatus : false
    }
    
    // Send data using Fetch API
    fetch(this.apiUrl, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem("token")}`
      },
      body: JSON.stringify(data),
      keepalive: true  
    }).catch(err => {
      console.error('Failed to send large data:', err);
    });
    
  }

}
