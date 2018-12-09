import { Rover } from "../ViewModel/Rover";
import { Camera } from "../ViewModel/Camera";

export class Photo {
  id: number;
  sol: number;
  img_src: string;
  earth_date: Date;
  rover: Rover[];
  camera: Camera[];

  constructor() {
  }
}
