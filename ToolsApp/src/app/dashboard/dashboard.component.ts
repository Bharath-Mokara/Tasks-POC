import { Component, OnInit } from '@angular/core';
import { Template } from '../models/template';
import { TemplateService } from '../Services/template.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit{

  templates: Template[] = [];

  constructor(private templateService: TemplateService, private router:Router) {
    
  }

  ngOnInit(): void {
    this.getAllTemplates();
  }


  getAllTemplates() : any
  {
    this.templateService.getTemplates().subscribe({
      next:(response:any)=>{
        this.templates = response;
        console.log(this.templates);
      }
    })
  }

  viewTemplate(templateContent:string,templateName:string)
  {
    console.log(templateContent);
    this.router.navigate(['/tools'], { queryParams: { content: templateContent, name: templateName } });
  }

  deleteTemplate(id: string)
  {
    this.templateService.deleteTemplate(id).subscribe({
      next:(response:any)=>
      {
        console.log("template deleted successfully");
        const templateIndex = this.templates.findIndex(template => template.id === id);
        if(templateIndex != -1)
        {
          this.templates.splice(templateIndex,1);
        }
      }
    });
  }


  CreateTemplate(): void
  {
    this.router.navigate(["/tools"]);
  }


}
