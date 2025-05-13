import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NouveauteDetailsComponent } from './nouveaute-details.component';

describe('NouveauteDetailsComponent', () => {
  let component: NouveauteDetailsComponent;
  let fixture: ComponentFixture<NouveauteDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NouveauteDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NouveauteDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
