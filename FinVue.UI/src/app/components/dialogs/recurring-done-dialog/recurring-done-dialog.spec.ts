import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecurringDoneDialog } from './recurring-done-dialog';

describe('RecurringDoneDialog', () => {
  let component: RecurringDoneDialog;
  let fixture: ComponentFixture<RecurringDoneDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecurringDoneDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecurringDoneDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
