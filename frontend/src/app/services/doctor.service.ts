import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse';
import { Doctor } from '../models/doctor';

@Injectable({
    providedIn: 'root'
})
export class DoctorService{
    private apiUrl = 'https://localhost:51174/Doctors'; 
    constructor(private http: HttpClient){}

      getDoctor(): Observable<ApiResponse<Doctor[]>>{
    return this.http.get<ApiResponse<Doctor[]>>(this.apiUrl);
  }
}