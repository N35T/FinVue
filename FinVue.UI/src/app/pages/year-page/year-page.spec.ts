import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YearPage } from './year-page';

describe('YearPage', () => {
  let component: YearPage;
  let fixture: ComponentFixture<YearPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [YearPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(YearPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
