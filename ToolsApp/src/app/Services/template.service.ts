import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Template } from '../models/template';

@Injectable({
  providedIn: 'root'
})
export class TemplateService {

  constructor(private httpclient: HttpClient) { }

  apiBaseAddress = environment.apiBaseAddress;

  //Adding the final html to the database
  addTemplate(data: any): Observable<any>{
    return this.httpclient.post(`${this.apiBaseAddress}api/Template`,data);
  }

  //getting all the templates 
  getTemplates():Observable<any>{
    return this.httpclient.get(`${this.apiBaseAddress}api/Template`);
  }

  //deleting a template based on its id.
  deleteTemplate(id: string): Observable<any>{
    return this.httpclient.delete(`${this.apiBaseAddress}api/Template/${id}`);
  }
}
