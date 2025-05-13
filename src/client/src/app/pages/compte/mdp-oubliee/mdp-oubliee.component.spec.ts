import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MdpOublieeComponent } from './mdp-oubliee.component';

describe('MdpOublieeComponent', () => {
  let component: MdpOublieeComponent;
  let fixture: ComponentFixture<MdpOublieeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MdpOublieeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MdpOublieeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
