import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MembersComponent } from './members.component';
import { WorkspaceService } from '../../core/services/workspace.service';
import { of, throwError } from 'rxjs';
import { TenantRole } from '../../core/models/organization.model';
import { Member } from '../../core/models/member.model';

describe('MembersComponent', () => {
  let component: MembersComponent;
  let fixture: ComponentFixture<MembersComponent>;
  let workspaceServiceSpy: jasmine.SpyObj<WorkspaceService>;

  const mockMembers: Member[] = [
    {
      id: 'user-1',
      email: 'admin@example.com',
      firstName: 'Alice',
      lastName: 'Admin',
      role: TenantRole.Admin,
      joinedAt: new Date().toISOString()
    },
    {
      id: 'user-2',
      email: 'agent@example.com',
      firstName: 'Bob',
      lastName: 'Smith',
      role: TenantRole.Agent,
      joinedAt: new Date().toISOString()
    }
  ];

  beforeEach(async () => {
    workspaceServiceSpy = jasmine.createSpyObj('WorkspaceService', ['getMembers']);
    workspaceServiceSpy.getMembers.and.returnValue(of(mockMembers));

    await TestBed.configureTestingModule({
      declarations: [MembersComponent],
      providers: [{ provide: WorkspaceService, useValue: workspaceServiceSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(MembersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load members on init', () => {
    expect(component.members.length).toBe(2);
    expect(component.isLoading).toBeFalse();
  });

  it('should show error message on load failure', () => {
    workspaceServiceSpy.getMembers.and.returnValue(throwError(() => new Error('Network error')));
    component.loadMembers();
    expect(component.errorMessage).toBeTruthy();
  });

  it('should return correct role label', () => {
    expect(component.getRoleLabel(TenantRole.Admin)).toBe('Admin');
    expect(component.getRoleLabel(TenantRole.Manager)).toBe('Manager');
    expect(component.getRoleLabel(TenantRole.Agent)).toBe('Agent');
  });

  it('should return correct initials', () => {
    expect(component.getInitials(mockMembers[0])).toBe('AA');
    expect(component.getInitials(mockMembers[1])).toBe('BS');
  });
});
