import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransactionTable } from './transaction-table';

describe('TransactionTable', () => {
  let component: TransactionTable;
  let fixture: ComponentFixture<TransactionTable>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TransactionTable]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TransactionTable);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
