import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserSettingsDataComponent } from './user-settings-data.component';

describe('UserSettingsDataComponent', () => {
  let component: UserSettingsDataComponent;
  let fixture: ComponentFixture<UserSettingsDataComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserSettingsDataComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserSettingsDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
