import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Patient } from '../models/patient';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse';

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private apiUrl = 'https://localhost:51174/patients';
  constructor(private http: HttpClient) { }

  getPatients(): Observable<ApiResponse<Patient[]>> {
      return this.http.get<ApiResponse<Patient[]>>(this.apiUrl);
  }

}
