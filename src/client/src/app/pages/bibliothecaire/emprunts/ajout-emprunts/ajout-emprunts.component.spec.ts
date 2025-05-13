import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AjoutEmpruntsComponent } from './ajout-emprunts.component';

describe('AjoutEmpruntsComponent', () => {
  let component: AjoutEmpruntsComponent;
  let fixture: ComponentFixture<AjoutEmpruntsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AjoutEmpruntsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AjoutEmpruntsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
