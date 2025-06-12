import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { baseurl } from '../../baseURL';

@Injectable({
  providedIn: 'root'
})
export class DesignationService {
  constructor(private httpClient : HttpClient) { }
 
    //register 
  public getDesignations(){
    return this.httpClient.get(`${baseurl}/Designation`);
  }

  public addDesignations(desig : any) {
    return this.httpClient.post(`${baseurl}/Designation`,desig,{
    responseType: 'text' // telling Angular not to parse as JSON
  });
  }

  public updateDesignations(desig : any, desigId : any){
    return this.httpClient.put(`${baseurl}/Designation/${desigId}`,desig,{
    responseType: 'text'
  });
  }

  public deleteDesignations(desigId : any){
    return this.httpClient.delete(`${baseurl}/Designation/${desigId}`,{
    responseType: 'text' 
  });
  }
}
