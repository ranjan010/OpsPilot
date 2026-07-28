import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TenantService } from '../../core/services/tenant.service';

@Component({
  selector: 'app-workspace-setup',
  standalone: false,
  templateUrl: './workspace-setup.component.html',
  styleUrls: ['./workspace-setup.component.scss']
})
export class WorkspaceSetupComponent implements OnInit {
  setupForm: FormGroup;
  isLoading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private tenantService: TenantService,
    private router: Router
  ) {
    this.setupForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      slug: ['', [Validators.required, Validators.pattern('^[a-z0-9-]+$')]]
    });
  }

  ngOnInit(): void {}

  onNameInput(): void {
    const nameVal = this.setupForm.get('name')?.value || '';
    const generatedSlug = nameVal
      .toLowerCase()
      .trim()
      .replace(/[^a-z0-9 -]/g, '')
      .replace(/\s+/g, '-')
      .replace(/-+/g, '-');
    this.setupForm.get('slug')?.setValue(generatedSlug, { emitEvent: false });
  }

  onSubmit(): void {
    if (this.setupForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    this.tenantService.createOrganization(this.setupForm.value).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err?.error?.message || 'Failed to create workspace. Please try again.';
      }
    });
  }
}
