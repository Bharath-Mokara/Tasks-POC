import { Component } from '@angular/core';

@Component({
  selector: 'app-paragraph-builder',
  templateUrl: './paragraph-builder.component.html',
  styleUrls: ['./paragraph-builder.component.css']
})
export class ParagraphBuilderComponent {

  dragTools: any[] = [
    {
      type:'textbox',
      content:`<input type="text">`
    },
    {
      type:'textarea',
      content:`<textarea><textarea>`
    },
    {
      type:'datepicker',
      content:`<input type="date">`
    }
  ];

  dropTools: any[] = []



  onDrop(event:any, dropTools: any[])
  {
    console.log(event.data);
  }

}
