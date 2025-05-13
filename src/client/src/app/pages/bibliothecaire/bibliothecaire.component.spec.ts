import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BibliothecaireComponent } from './bibliothecaire.component';

describe('BibliothecaireComponent', () => {
  let component: BibliothecaireComponent;
  let fixture: ComponentFixture<BibliothecaireComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BibliothecaireComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BibliothecaireComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
