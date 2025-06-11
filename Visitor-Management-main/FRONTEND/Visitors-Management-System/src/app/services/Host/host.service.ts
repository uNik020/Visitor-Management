import { Injectable } from '@angular/core';
import { baseurl } from '../../baseURL';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HostService {
 constructor(private httpClient: HttpClient) {}

  // Get all hosts
  public getHosts(): Observable<any> {
    return this.httpClient.get(`${baseurl}/Host`);
  }

  // Add new host
  public addHost(host: any): Observable<any> {
    return this.httpClient.post(`${baseurl}/Host`, host, {
      responseType: 'text'
    });
  }

  // Update existing host
  public updateHost(host: any, hostId: any): Observable<any> {
    return this.httpClient.put(`${baseurl}/Host/${hostId}`, host, {
      responseType: 'text'
    });
  }

  // Delete host
  public deleteHost(hostId: any): Observable<any> {
    return this.httpClient.delete(`${baseurl}/Host/${hostId}`, {
      responseType: 'text'
    });
  }
}