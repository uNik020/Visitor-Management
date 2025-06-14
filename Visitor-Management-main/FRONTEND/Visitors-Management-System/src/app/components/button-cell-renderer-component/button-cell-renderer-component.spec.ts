import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ButtonCellRendererComponent } from './button-cell-renderer-component';

describe('ButtonCellRendererComponent', () => {
  let component: ButtonCellRendererComponent;
  let fixture: ComponentFixture<ButtonCellRendererComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ButtonCellRendererComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ButtonCellRendererComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
