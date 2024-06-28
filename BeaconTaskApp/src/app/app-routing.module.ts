import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ResourceComponent } from './resource/resource.component';
import { LoginComponent } from './login/login.component';
import { AboutComponent } from './about/about.component';


const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch:'full'},
  { path: 'login', component: LoginComponent},
  { path: 'resource', component: ResourceComponent },
  { path: 'about', component: AboutComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }