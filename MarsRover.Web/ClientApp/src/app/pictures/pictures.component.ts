import { Component, OnInit } from '@angular/core';
import { DataService } from '../data.service';
import { Observable } from 'rxjs';
import { ActivatedRoute } from "@angular/router";
import { NgxSpinnerService } from 'ngx-spinner';

import { Rover } from '../ViewModel/Rover';
import { Camera } from '../ViewModel/Camera';
import { Photo } from '../ViewModel/Photo';
import { EarthDate } from '../ViewModel/EarthDate';

@Component({
  selector: 'app-pictures',
  templateUrl: './pictures.component.html',
  styleUrls: ['./pictures.component.scss']
})
export class PicturesComponent implements OnInit {
  minDate: EarthDate;
  maxDate: EarthDate;
  selectedDate: EarthDate;
  rover: Rover;
  photos: Photo[];

  constructor(private route: ActivatedRoute, private data: DataService, private spinner: NgxSpinnerService) {
    this.route.params.subscribe(params => this.rover = params.id);
  }

  ngOnInit() {
    this.spinner.show();
    this.data.getRover(this.rover).subscribe(
      data => {
        console.log(data);
        this.rover = data as Rover;

        let max_date = new Date(this.rover.max_date);
        let min_date = new Date(this.rover.landing_date);
        console.log("min_date: " + this.rover.landing_date);
        console.log("max_date: " + this.rover.max_date);

        this.minDate = {
          year: min_date.getFullYear(),
          month: min_date.getMonth() + 1,
          day: min_date.getDate()
        };
        this.maxDate = {
          year: max_date.getFullYear(),
          month: max_date.getMonth() + 1,
          day: max_date.getDate()
        };
        this.selectedDate = {
          year: max_date.getFullYear(),
          month: max_date.getMonth() + 1,
          day: max_date.getDate()
        };

        this.ShowPictures(null, this.selectedDate);

        this.spinner.hide();
      }
    );
  }

  public ShowPictures(event, date: EarthDate) {
    this.spinner.show();
    console.log("date: ", date);
    console.log("rover: ", this.rover);
    this.data.getRoverPhotos(this.rover.id, date).subscribe(
      data => {
        this.photos = data as Photo[];
        this.spinner.hide();
      }
    );
  }

  public GetHttpsSrc(img_src: string)
  {
    return img_src.replace(/http:\/\//gi, "https:\/\/");
  }
}
