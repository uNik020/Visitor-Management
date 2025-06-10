import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageHosts } from './manage-hosts';

describe('ManageHosts', () => {
  let component: ManageHosts;
  let fixture: ComponentFixture<ManageHosts>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageHosts]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageHosts);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
