import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SideNav } from './side-nav';

describe('SideNav', () => {
  let component: SideNav;
  let fixture: ComponentFixture<SideNav>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SideNav]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SideNav);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
