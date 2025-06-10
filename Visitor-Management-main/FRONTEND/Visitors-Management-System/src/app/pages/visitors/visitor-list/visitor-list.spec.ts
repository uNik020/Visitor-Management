import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VisitorList } from './visitor-list';

describe('VisitorList', () => {
  let component: VisitorList;
  let fixture: ComponentFixture<VisitorList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VisitorList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VisitorList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
