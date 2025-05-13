import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MoifierNouveauteComponent } from './modifier-nouveaute.component';

describe('MoifierNouveauteComponent', () => {
  let component: MoifierNouveauteComponent;
  let fixture: ComponentFixture<MoifierNouveauteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MoifierNouveauteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MoifierNouveauteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
