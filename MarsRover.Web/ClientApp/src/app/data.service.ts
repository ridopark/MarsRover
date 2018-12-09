import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EarthDate } from "./ViewModel/EarthDate";

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor(private http: HttpClient) { }

  

  getRovers() {
    return this.http.get('https://localhost:44300/api/marsrover/rovers');
  }

  getRover(roverId) {
    return this.http.get('https://localhost:44300/api/marsrover/rovers/' + roverId);
  }

  getRoverPhotos(roverId: number, earthDate: EarthDate) {
    console.log(earthDate);
    let date = earthDate.year + "-" + earthDate.month + "-" + earthDate.day;
    return this.http.get('https://localhost:44300/api/marsrover/rovers/' + roverId + '/earthdate/' + date + '/photos');
  }

}
