import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RoversComponent } from './rovers.component';

describe('RoversComponent', () => {
  let component: RoversComponent;
  let fixture: ComponentFixture<RoversComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RoversComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoversComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
