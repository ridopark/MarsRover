import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoversComponent } from './rovers/rovers.component';
import { DetailsComponent } from './details/details.component';
import { PicturesComponent } from './pictures/pictures.component';

const routes: Routes = [
  {
    path: '',
    component: RoversComponent
  },
  {
    path: 'details/:id',
    component: DetailsComponent
  },
  {
    path: 'pictures/rover/:id',
    component: PicturesComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
