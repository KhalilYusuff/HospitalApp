import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PatientListComponent } from './components/patient/patient-list/patient-list.component';
import { DoctorListComponent } from './components/doctor/doctor-list/doctor-list.component';

const routes: Routes = [

 {
    path: '',
    component: PatientListComponent
  }, 
  {
    path: 'patients', 
    component: PatientListComponent
  }, 
  {
    path: 'doctors', 
    component: DoctorListComponent
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
