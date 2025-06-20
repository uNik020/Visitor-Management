import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { baseurl } from '../../baseURL';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  constructor(private http: HttpClient) {}

  // Create appointment
  public createAppointment(appointment: any): Observable<any> {
    return this.http.post(`${baseurl}/Appointments`, appointment, {
      responseType: 'text'
    });
  }

  // Get all appointments
  public getAllAppointments(): Observable<any> {
    return this.http.get(`${baseurl}/Appointments`);
  }

  // Get appointments by host
  public getAppointmentsByHost(hostId: number): Observable<any> {
    return this.http.get(`${baseurl}/Appointments/host/${hostId}`);
  }

  // Update status (approve/reject/checkin/checkout)
  public updateAppointmentStatus(visitId: number, statusDto: any): Observable<any> {
    return this.http.put(`${baseurl}/Appointments/${visitId}/status`, statusDto, {
      responseType: 'text'
    });
  }
}
