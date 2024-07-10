import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ToolComponent } from './tool/tool.component';
import { DashboardComponent } from './dashboard/dashboard.component';


const routes: Routes = [
  {path:'',redirectTo:"/home",pathMatch:'full'},
  {path:'dashboard', component:DashboardComponent},
  {path:'home',component:HomeComponent},
  {path:'tools',component:ToolComponent},
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
