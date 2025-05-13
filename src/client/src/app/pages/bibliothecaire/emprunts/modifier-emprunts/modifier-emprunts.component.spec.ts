import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModifierEmpruntsComponent } from './modifier-emprunts.component';

describe('ModifierEmpruntsComponent', () => {
  let component: ModifierEmpruntsComponent;
  let fixture: ComponentFixture<ModifierEmpruntsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModifierEmpruntsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModifierEmpruntsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
