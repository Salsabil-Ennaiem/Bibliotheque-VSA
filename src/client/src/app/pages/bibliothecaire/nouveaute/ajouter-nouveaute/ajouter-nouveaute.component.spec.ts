import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AjouterNouveauteComponent } from './ajouter-nouveaute.component';

describe('AjouterNouveauteComponent', () => {
  let component: AjouterNouveauteComponent;
  let fixture: ComponentFixture<AjouterNouveauteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AjouterNouveauteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AjouterNouveauteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
