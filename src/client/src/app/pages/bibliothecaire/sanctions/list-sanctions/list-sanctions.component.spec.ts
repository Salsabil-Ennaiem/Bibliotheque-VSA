import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListSanctionsComponent } from './list-sanctions.component';

describe('ListSanctionsComponent', () => {
  let component: ListSanctionsComponent;
  let fixture: ComponentFixture<ListSanctionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListSanctionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListSanctionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
