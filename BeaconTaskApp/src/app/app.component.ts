import { Component, HostListener } from '@angular/core';
import { LockService } from './Services/lock-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'BeaconTaskApp';

  userId = 'bharath001'; 

  constructor(private lockService: LockService) {}

  ngOnInit() {
    // Attach the 'beforeunload' event listener
    window.addEventListener('beforeunload', this.handleBeforeUnload);
    
    this.lockService.sendUserExitData(this.userId);
  }

  ngOnDestroy() {
    // Remove the 'beforeunload' event listener
    window.removeEventListener('beforeunload', this.handleBeforeUnload);
  }

  private handleBeforeUnload = (event: Event) => {
    this.lockService.sendUserExitData(this.userId);
  }
}


