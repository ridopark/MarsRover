import { Component, OnInit } from '@angular/core';
import { DataService } from '../data.service';
import { Observable } from 'rxjs';
import { Camera } from '../ViewModel/Camera';

@Component({
  selector: 'app-rovers',
  templateUrl: './rovers.component.html',
  styleUrls: ['./rovers.component.scss']
})
export class RoversComponent implements OnInit {
  rovers$: Object;

  constructor(private data: DataService) { }

  ngOnInit() {
    this.data.getRovers().subscribe(
      data => {
        this.rovers$ = data;
      }
    );
  }

  getCameraList(cameraArr: Camera[]) {
    return cameraArr.map(x => x.name).join(", ");
  }
}
