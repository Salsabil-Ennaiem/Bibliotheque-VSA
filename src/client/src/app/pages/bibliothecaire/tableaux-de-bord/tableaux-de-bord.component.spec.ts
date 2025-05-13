import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableauxDeBordComponent } from './tableaux-de-bord.component';

describe('TableauxDeBordComponent', () => {
  let component: TableauxDeBordComponent;
  let fixture: ComponentFixture<TableauxDeBordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TableauxDeBordComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TableauxDeBordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
