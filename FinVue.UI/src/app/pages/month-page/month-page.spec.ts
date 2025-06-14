import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthPage } from './month-page';

describe('MonthPage', () => {
  let component: MonthPage;
  let fixture: ComponentFixture<MonthPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MonthPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MonthPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
