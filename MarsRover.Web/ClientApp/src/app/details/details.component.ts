import { Component, OnInit } from '@angular/core';
import { DataService } from '../data.service';
import { Observable } from 'rxjs';
import { ActivatedRoute } from "@angular/router";
import { Camera } from '../ViewModel/Camera';
import { Rover } from '../ViewModel/Rover';

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.scss']
})
export class DetailsComponent implements OnInit {

  rover$: Rover;

  constructor(private route: ActivatedRoute, private data: DataService) {
    this.route.params.subscribe(params => this.rover$ = params.id);
  }

  ngOnInit() {
    this.data.getRover(this.rover$).subscribe(
      data => {
        this.rover$ = data as Rover;
      }
    );
  }

  getCameraList(cameraArr: Camera[]) {
    if (cameraArr !== undefined) {
      return cameraArr.map(x => x.name).join(", ");
    }
  }

}
