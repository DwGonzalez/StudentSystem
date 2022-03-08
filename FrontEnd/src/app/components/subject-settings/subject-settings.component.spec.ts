import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectSettingsComponent } from './subject-settings.component';

describe('SubjectSettingsComponent', () => {
  let component: SubjectSettingsComponent;
  let fixture: ComponentFixture<SubjectSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubjectSettingsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubjectSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
