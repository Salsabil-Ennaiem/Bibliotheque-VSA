import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModifierLivresComponent } from './modifier-livres.component';

describe('ModifierLivresComponent', () => {
  let component: ModifierLivresComponent;
  let fixture: ComponentFixture<ModifierLivresComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModifierLivresComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModifierLivresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
