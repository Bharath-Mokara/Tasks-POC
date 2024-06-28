import { Component, OnDestroy, OnInit } from '@angular/core';
import { LockService } from '../services/lock-service.service';

@Component({
  selector: 'app-resource',
  templateUrl: './resource.component.html',
  styleUrls: ['./resource.component.css']
})
export class ResourceComponent implements OnInit, OnDestroy{

  constructor(private lockService: LockService){

  }

  ngOnInit(): void {
    //Adding the unload eventlistner during initialization of resource component
    window.addEventListener('beforeunload', this.releaseLockOnUnload);
  }

  ngOnDestroy(): void {
    //removing the unload eventlistner during destroy of resource component
    window.removeEventListener('beforeunload', this.releaseLockOnUnload);
  }

  private releaseLockOnUnload = (event: Event) => {
    this.lockService.sendUserExitData();
  }




}
