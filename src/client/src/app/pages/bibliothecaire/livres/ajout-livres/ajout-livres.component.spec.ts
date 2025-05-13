import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AjoutLivresComponent } from './ajout-livres.component';

describe('AjoutLivresComponent', () => {
  let component: AjoutLivresComponent;
  let fixture: ComponentFixture<AjoutLivresComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AjoutLivresComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AjoutLivresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
