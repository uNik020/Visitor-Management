import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageDepartments } from './manage-departments';

describe('ManageDepartments', () => {
  let component: ManageDepartments;
  let fixture: ComponentFixture<ManageDepartments>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageDepartments]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageDepartments);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
