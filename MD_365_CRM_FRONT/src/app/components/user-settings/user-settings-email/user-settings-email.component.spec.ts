import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserSettingsEmailComponent } from './user-settings-email.component';

describe('UserSettingsEmailComponent', () => {
  let component: UserSettingsEmailComponent;
  let fixture: ComponentFixture<UserSettingsEmailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserSettingsEmailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserSettingsEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
