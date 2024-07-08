import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SampleComponent } from './sample/sample.component';
import { HomeComponent } from './home/home.component';
import { ToolComponent } from './tool/tool.component';

const routes: Routes = [
  {path:'',redirectTo:"/home",pathMatch:'full'},
  {path:'home',component:HomeComponent},
  {path:'sample',component:SampleComponent},
  {path:'tools',component:ToolComponent},
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
