import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageDesignations } from './manage-designations';

describe('ManageDesignations', () => {
  let component: ManageDesignations;
  let fixture: ComponentFixture<ManageDesignations>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageDesignations]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageDesignations);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
