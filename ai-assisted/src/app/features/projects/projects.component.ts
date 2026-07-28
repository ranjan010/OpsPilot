import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { WorkspaceService } from '../../core/services/workspace.service';
import { Project } from '../../core/models/project.model';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <section class="page-card">
      <h2>Projects</h2>
      <p>Create and manage your tenant projects from here.</p>

      <form [formGroup]="form" (ngSubmit)="submit()" class="project-form">
        <input formControlName="name" placeholder="Project name" />
        <input formControlName="key" placeholder="Project key" />
        <textarea formControlName="description" placeholder="Project description"></textarea>
        <button type="submit" [disabled]="form.invalid">Create project</button>
      </form>

      <div class="list" *ngIf="projects.length; else emptyState">
        <div class="item" *ngFor="let project of projects">
          <strong>{{ project.name }}</strong>
          <span>{{ project.key }}</span>
          <small>{{ project.description }}</small>
        </div>
      </div>
      <ng-template #emptyState>
        <p class="empty">No projects yet for this workspace.</p>
      </ng-template>
    </section>
  `,
  styles: [
    `:host { display: block; }
     .page-card { background: rgba(15, 23, 42, 0.72); border: 1px solid rgba(255,255,255,0.12); border-radius: 16px; padding: 1.5rem; color: #f8fafc; }
     h2 { margin-top: 0; }
     .project-form { display: grid; gap: 0.75rem; margin-bottom: 1rem; }
     input, textarea, button { border-radius: 8px; border: 1px solid rgba(255,255,255,0.12); padding: 0.7rem 0.8rem; background: rgba(15,23,42,0.6); color: #fff; }
     button { cursor: pointer; background: linear-gradient(135deg, #6366f1, #4f46e5); }
     .item { display: grid; gap: 0.25rem; padding: 0.75rem 0; border-bottom: 1px solid rgba(255,255,255,0.1); }
     .empty { color: #94a3b8; }`
  ]
})
export class ProjectsComponent implements OnInit {
  projects: Project[] = [];
  form: FormGroup;

  constructor(private workspaceService: WorkspaceService, private fb: FormBuilder) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      key: ['', [Validators.required, Validators.pattern('^[A-Z0-9-]+$')]],
      description: ['']
    });
  }

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.workspaceService.getProjects().subscribe(projects => this.projects = projects);
  }

  submit(): void {
    if (this.form.invalid) return;
    this.workspaceService.createProject(this.form.value).subscribe(() => {
      this.form.reset();
      this.loadProjects();
    });
  }
}
