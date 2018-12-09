import { Camera } from "../ViewModel/Camera";

export class Rover {
  id: number;
  name: string;
  landing_date: Date;
  launch_date: Date;
  status: string;
  max_sol: number;
  max_date: Date;
  total_photos: number;
  cameras: Camera[];

  constructor() {
  }
}
