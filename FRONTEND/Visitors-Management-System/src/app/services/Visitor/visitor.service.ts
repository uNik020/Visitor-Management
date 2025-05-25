import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { baseurl } from '../../baseURL';

@Injectable({
  providedIn: 'root'
})
export class VisitorService {

  constructor(private httpClient: HttpClient) {}

  // Get all hosts
  public getVisitor(): Observable<any> {
    return this.httpClient.get(`${baseurl}/Visitor`);
  }

  // Add new host
  public addVisitor(visitor: any): Observable<any> {
    return this.httpClient.post(`${baseurl}/Visitor`,visitor, {
      responseType: 'text'
    });
  }

  public updateVisitor(visitor: any, visitorId: any): Observable<any> {
    return this.httpClient.put(`${baseurl}/Visitor/${visitorId}`, visitor, {
      responseType: 'text'
    });
  }

  // Delete visitor
  public deleteVisitor(visitorId: any): Observable<any> {
    return this.httpClient.delete(`${baseurl}/Visitor/${visitorId}`, {
      responseType: 'text'
    });
  }
}
