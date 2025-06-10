import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { baseurl } from '../../baseURL';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  constructor(private httpClient : HttpClient) { }
 
    //register 
  public getDepartments(){
    return this.httpClient.get(`${baseurl}/Department`);
  }

  public addDepartments(dept : any) {
    return this.httpClient.post(`${baseurl}/Department`,dept,{
    responseType: 'text' // 👈 tell Angular not to parse as JSON
  });
  }

  public updateDepartments(dept : any, deptId : any){
    return this.httpClient.put(`${baseurl}/Department/${deptId}`,dept,{
    responseType: 'text' // 👈 tell Angular not to parse as JSON
  });
  }

  public deleteDepartments(deptId : any){
    return this.httpClient.delete(`${baseurl}/Department/${deptId}`,{
    responseType: 'text' // 👈 tell Angular not to parse as JSON
  });
  }
}
