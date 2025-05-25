import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AjoutSanctionComponent } from './ajout-sanction.component';

describe('AjoutSanctionComponent', () => {
  let component: AjoutSanctionComponent;
  let fixture: ComponentFixture<AjoutSanctionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AjoutSanctionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AjoutSanctionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
