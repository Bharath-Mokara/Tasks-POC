import { JsonPipe } from '@angular/common';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LockService {

  constructor() { }
  private readonly apiUrl = 'http://localhost:5140/api/Sample/release-Access';

  sendUserExitData(userId: string) {

    const releaseData = {
      id: userId,
      message: "User lock released",
      lockStatus : false
    };

    // Use the Beacon API to send the data
    const blob = new Blob([JSON.stringify(releaseData)], { type: 'application/json' });
    navigator.sendBeacon(this.apiUrl,blob);

    // var data = {
    //   id: userId,
    //   message: "User lock released",
    //   lockStatus : false
    // }
    
    // // Send data using Fetch API
    // fetch(this.apiUrl, {
    //   method: 'POST',
    //   headers: {
    //     'Content-Type': 'application/json'
    //   },
    //   body: JSON.stringify(data),
    //   keepalive: true  
    // }).catch(err => {
    //   console.error('Failed to send large data:', err);
    // });
    
  }

}
