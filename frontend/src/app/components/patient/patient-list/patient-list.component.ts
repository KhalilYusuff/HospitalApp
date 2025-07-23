import { Component } from '@angular/core';
import { PatientService } from '../../../services/patient.service';
import { Patient } from '../../../models/patient';
import { OnInit } from '@angular/core';



@Component({
  selector: 'app-patient-list',
  standalone: false,
  templateUrl: './patient-list.html',
  styleUrl: './patient-list.css'
})
export class PatientListComponent implements OnInit {
  patients: Patient[] = [];

  constructor(private patientService: PatientService) { }

  ngOnInit(): void {
    this.patientService.getPatients().subscribe({
      next: (response) => {
        if (response.isSuccess){
          this.patients = response.result;
        }
      },
      error: (error) => {
        console.error('Error while getting patients ', error);
      }
    })
  }

}
