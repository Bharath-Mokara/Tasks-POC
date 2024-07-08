
import { Component, OnInit } from '@angular/core';
import { DndDropEvent,DropEffect} from 'ngx-drag-drop';
// import { field, value } from '../global.model';
import { ActivatedRoute } from '@angular/router';
// import swal from 'sweetalert2';

@Component({
  selector: 'app-sample',
  templateUrl: './sample.component.html',
  styleUrls: ['./sample.component.css']
})
export class SampleComponent implements OnInit {

  value:any={
    label:"",
    value:""
  };
  success = false;

  fieldModels:Array<any>=[
    {
      "type": "text",
      "icon": "fa-font",
      "label": "Text",
      "description": "Enter your name",
      "placeholder": "Enter your name",
      "className": "form-control",
      "subtype": "text",
      "regex" : "",
      "handle":true
    },
    {
      "type": "email",
      "icon": "fa-envelope",
      "required": true,
      "label": "Email",
      "description": "Enter your email",
      "placeholder": "Enter your email",
      "className": "form-control",
      "subtype": "text",
      "regex" : "^([a-zA-Z0-9_.-]+)@([a-zA-Z0-9_.-]+)\.([a-zA-Z]{2,5})$",
      "errorText": "Please enter a valid email",
      "handle":true
    },
    {
      "type": "number",
      "label": "Number",
      "icon": "fa-html5",
      "description": "Age",
      "placeholder": "Enter your age",
      "className": "form-control",
      "value": "20",
      "min": 12,
      "max": 90
    },
    {
      "type": "textarea",
      "icon":"fa-text-width",
      "label": "Textarea" 
    },
  ];

  modelFields:Array<any>=[];

  model:any = {
    name:'App name...',
    description:'App Description...',
    theme:{
      bgColor:"ffffff",
      textColor:"555555",
      bannerImage:""
    },
    attributes:this.modelFields
  };

  report = false;
  reports:any = [];

  constructor(
    private route:ActivatedRoute
  ) { }

  ngOnInit() {
    // this.route.params.subscribe( params =>{
    //   console.log(params);
    //   this.us.getDataApi('/admin/getFormById',{id:params.id}).subscribe(r=>{
    //     console.log(r);
    //     this.model = r['data'];
    //   });
    // });


    // this.model = this.cs.data; 
    // console.log(this.model.data);

  }

  onDragStart(event:DragEvent) {
    console.log("drag started", JSON.stringify(event, null, 2));
  }
  
  onDragEnd(event:DragEvent) {
    console.log("drag ended", JSON.stringify(event, null, 2));
  }
  
  onDraggableCopied(event:DragEvent) {
    console.log("draggable copied", JSON.stringify(event, null, 2));
  }
  
  onDraggableLinked(event:DragEvent) {
    console.log("draggable linked", JSON.stringify(event, null, 2));
  }
    
   onDragged( item:any, list:any[], effect:DropEffect ) {
    if( effect === "move" ) {
      const index = list.indexOf( item );
      list.splice( index, 1 );
    }
  }
      
  onDragCanceled(event:DragEvent) {
    console.log("drag cancelled", JSON.stringify(event, null, 2));
  }
  
  onDragover(event:DragEvent) {
    console.log("dragover", JSON.stringify(event, null, 2));
  }
  
  onDrop( event:DndDropEvent, list?:any[] ) {
    if( list && (event.dropEffect === "copy" || event.dropEffect === "move") ) {
      
      if(event.dropEffect === "copy")     
      event.data.name = event.data.type+'-'+new Date().getTime();
      let index = event.index;
      if( typeof index === "undefined" ) {
        index = list.length;
      }
      list.splice( index, 0, event.data );
    }
  }

  addValue(values:any){
    values.push(this.value);
    this.value={label:"",value:""};
  }

  removeField(i : any){
    // swal({
    //   title: 'Are you sure?',
    //   text: "Do you want to remove this field?",
    //   type: 'warning',
    //   showCancelButton: true,
    //   confirmButtonColor: '#00B96F',
    //   cancelButtonColor: '#d33',
    //   confirmButtonText: 'Yes, remove!'
    // }).then((result) => {
    //   if (result.value) {
    //     this.model.attributes.splice(i,1);
    //   }
    // });

    this.model.attributes.splice(i,1);

  }

  updateForm(){
    let input = new FormData;
    input.append('id',this.model._id);
    input.append('name',this.model.name);
    input.append('description',this.model.description);
    input.append('bannerImage',this.model.theme.bannerImage);
    input.append('bgColor',this.model.theme.bgColor);
    input.append('textColor',this.model.theme.textColor);
    input.append('attributes',JSON.stringify(this.model.attributes));

    // this.us.putDataApi('/admin/updateForm',input).subscribe(r=>{
    //   console.log(r);
    //   swal('Success','App updated successfully','success');
    // });
  }


  initReport(){
    this.report = true; 
    let input = {
      id:this.model._id
    }
    // this.us.getDataApi('/admin/allFilledForms',input).subscribe(r=>{
    //   this.reports = r.data;
    //   console.log('reports',this.reports);
    //   this.reports.map(records=>{
    //     return records.attributes.map(record=>{
    //       if(record.type=='checkbox'){
    //         record.value = record.values.filter(r=>r.selected).map(i=>i.value).join(',');
    //       }
    //     })
    //   });
    // });
  }



  toggleValue(item:any){
    item.selected = !item.selected;
  }

  submit(): boolean {
    let valid = true;
    let validationArray = JSON.parse(JSON.stringify(this.model.attributes));
    validationArray.reverse();
  
    for (const field of validationArray) {
      console.log(field.label + '=>' + field.required + '=>' + field.value);
  
      if (field.required && !field.value && field.type != 'checkbox') {
        // Swal.fire('Error', 'Please enter ' + field.label, 'error');
        valid = false;
        break;
      }
  
      if (field.required && field.regex) {
        let regex = new RegExp(field.regex);
        if (!regex.test(field.value)) {
          // Swal.fire('Error', field.errorText, 'error');
          valid = false;
          break;
        }
      }
  
      if (field.required && field.type == 'checkbox') {
        if (field.values.filter((r: any) => r.selected).length === 0) {
          // Swal.fire('Error', 'Please enter ' + field.label, 'error');
          valid = false;
          break;
        }
      }
    }
  
    if (!valid) {
      return false;
    }
  
    console.log('Save', this.model);
    let input = new FormData();
    input.append('formId', this.model._id);
    input.append('attributes', JSON.stringify(this.model.attributes));
  
    // Uncomment the following lines when using the actual API call
    // this.us.postDataApi('/user/formFill', input).subscribe(
    //   r => {
    //     console.log(r);
    //     Swal.fire('Success', 'You have contacted successfully', 'success');
    //     this.success = true;
    //   },
    //   error => {
    //     Swal.fire('Error', error.message, 'error');
    //   }
    // );
  
    return true;
  }
  
}