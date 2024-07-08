import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  myform = new FormGroup({});
  formControls:any = [];
  toolboxControls = [
    { type: 'textbox', label: 'Text Box' },
    { type: 'textarea', label: 'Text Area' },
    { type: 'dropdown', label: 'Dropdown', options: [{ label: 'Option 1', value: 1 }, { label: 'Option 2', value: 2 }] }
  ];

  onDrop(event: any) {
    console.log("on drop function called",event);
    const control = { ...event.data, key: `control-${this.formControls.length}` };
    this.formControls.push(control);
    this.myform.addControl(control.key, new FormControl(''));
  }
}
