import { Component } from '@angular/core';
import { Doctor } from '../../../models/doctor';
import { DoctorService } from '../../../services/doctor.service';
import { OnInit } from '@angular/core';

@Component({
  selector: 'app-doctor-list',
  standalone: false,
  templateUrl: './doctor-list.html',
  styleUrl: './doctor-list.css'
})
export class DoctorListComponent implements OnInit{
  doctors: Doctor[] = [];

  constructor(private doctorService: DoctorService){}

  ngOnInit(): void {
    this.doctorService.getDoctor().subscribe({
      next:(response) =>{
        if (response.isSuccess){
          this.doctors = response.result;
        }
      },
      error: (error) =>{
        console.error('Error while getting doctors ', error);
      }
    })
  }
  
}
