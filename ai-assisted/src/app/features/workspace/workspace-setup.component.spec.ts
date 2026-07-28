import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { WorkspaceSetupComponent } from './workspace-setup.component';
import { WorkspaceSetupModule } from './workspace-setup.module';
import { TenantService } from '../../core/services/tenant.service';

describe('WorkspaceSetupComponent', () => {
  let fixture: ComponentFixture<WorkspaceSetupComponent>;
  let component: WorkspaceSetupComponent;
  let tenantService: jasmine.SpyObj<TenantService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const tenantServiceSpy = jasmine.createSpyObj('TenantService', ['createOrganization']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [WorkspaceSetupModule],
      providers: [
        { provide: TenantService, useValue: tenantServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(WorkspaceSetupComponent);
    component = fixture.componentInstance;
    tenantService = TestBed.inject(TenantService) as jasmine.SpyObj<TenantService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should keep the form invalid until required values are provided', () => {
    expect(component.setupForm.valid).toBeFalse();
  });

  it('should submit and navigate to the dashboard on success', () => {
    tenantService.createOrganization.and.returnValue(of({ id: '1', name: 'Acme', slug: 'acme', role: 0, joinedAt: '' } as any));

    component.setupForm.setValue({ name: 'Acme Support', slug: 'acme-support' });
    component.onSubmit();

    expect(tenantService.createOrganization).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/']);
  });
});
