import { Component } from '@angular/core';

@Component({
  selector: 'app-settings',
  standalone: true,
  template: `
    <section class="page-card">
      <h2>Settings</h2>
      <p>Adjust workspace and tenant settings from this area.</p>
    </section>
  `,
  styles: [
    `:host { display: block; }
     .page-card { background: rgba(15, 23, 42, 0.72); border: 1px solid rgba(255,255,255,0.12); border-radius: 16px; padding: 1.5rem; color: #f8fafc; }
     h2 { margin-top: 0; }`
  ]
})
export class SettingsComponent {}
