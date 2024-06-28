import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { LoginViewModel } from '../models/login-view-model';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  apiBaseAddress = environment.apiBaseAddress;
  isLoggedIn !: boolean

  constructor(private httpClient: HttpClient) { }

  public Login(loginViewModel: LoginViewModel) : Observable<any>
  {
    return this.httpClient.post<any>(`${this.apiBaseAddress}/api/Auth/login`,loginViewModel,{responseType:'json'});
  }

  public logout() : Observable<any>
  {
    return this.httpClient.get<any>(`${this.apiBaseAddress}/api/Auth/logout`,{responseType: 'json'});
  }
}
