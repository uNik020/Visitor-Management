import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VisitorDetails } from './visitor-details';

describe('VisitorDetails', () => {
  let component: VisitorDetails;
  let fixture: ComponentFixture<VisitorDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VisitorDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VisitorDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
