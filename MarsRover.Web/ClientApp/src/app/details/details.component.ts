import { Component, OnInit } from '@angular/core';
import { DataService } from '../data.service';
import { Observable } from 'rxjs';
import { ActivatedRoute } from "@angular/router";
import { Camera } from '../ViewModel/Camera';

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.scss']
})
export class DetailsComponent implements OnInit {

  rover$: Object;

  constructor(private route: ActivatedRoute, private data: DataService) {
     this.route.params.subscribe( params => this.rover$ = params.id );
  }

  ngOnInit() {
    this.data.getRover(this.rover$).subscribe(
      data => this.rover$ = data 
    );
  }

  getCameraList(cameraArr: Camera[]) {
    if (cameraArr !== undefined) {
      return cameraArr.map(x => x.name).join(", ");
    }
  }

}
