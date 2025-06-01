import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfitCard } from './profit-card';

describe('ProfitCard', () => {
  let component: ProfitCard;
  let fixture: ComponentFixture<ProfitCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfitCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfitCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
