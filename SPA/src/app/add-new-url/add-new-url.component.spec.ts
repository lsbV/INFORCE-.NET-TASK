import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddNewUrlComponent } from './add-new-url.component';

describe('AddNewUrlComponent', () => {
  let component: AddNewUrlComponent;
  let fixture: ComponentFixture<AddNewUrlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddNewUrlComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddNewUrlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
