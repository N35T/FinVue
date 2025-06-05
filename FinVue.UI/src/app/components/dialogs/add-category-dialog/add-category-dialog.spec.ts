import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCategoryDialog } from './add-category-dialog';

describe('AddCategoryDialog', () => {
  let component: AddCategoryDialog;
  let fixture: ComponentFixture<AddCategoryDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddCategoryDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddCategoryDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
