import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { WorkspaceService } from '../../core/services/workspace.service';
import { Ticket, TicketPriority } from '../../core/models/ticket.model';

@Component({
  selector: 'app-tickets',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <section class="page-card">
      <h2>Tickets</h2>
      <p>Track support requests and assignments for your workspace.</p>

      <form [formGroup]="form" (ngSubmit)="submit()" class="ticket-form">
        <input formControlName="title" placeholder="Ticket title" />
        <textarea formControlName="description" placeholder="Ticket description"></textarea>
        <select formControlName="priority">
          <option [ngValue]="TicketPriority.Low">Low</option>
          <option [ngValue]="TicketPriority.Medium">Medium</option>
          <option [ngValue]="TicketPriority.High">High</option>
          <option [ngValue]="TicketPriority.Urgent">Urgent</option>
        </select>
        <button type="submit" [disabled]="form.invalid">Create ticket</button>
      </form>

      <div class="list" *ngIf="tickets.length; else emptyState">
        <div class="item" *ngFor="let ticket of tickets">
          <strong>{{ ticket.title }}</strong>
          <span>{{ ticket.key }}</span>
          <small>{{ ticket.description }}</small>
        </div>
      </div>
      <ng-template #emptyState>
        <p class="empty">No tickets yet for this workspace.</p>
      </ng-template>
    </section>
  `,
  styles: [
    `:host { display: block; }
     .page-card { background: rgba(15, 23, 42, 0.72); border: 1px solid rgba(255,255,255,0.12); border-radius: 16px; padding: 1.5rem; color: #f8fafc; }
     h2 { margin-top: 0; }
     .ticket-form { display: grid; gap: 0.75rem; margin-bottom: 1rem; }
     input, textarea, select, button { border-radius: 8px; border: 1px solid rgba(255,255,255,0.12); padding: 0.7rem 0.8rem; background: rgba(15,23,42,0.6); color: #fff; }
     button { cursor: pointer; background: linear-gradient(135deg, #6366f1, #4f46e5); }
     .item { display: grid; gap: 0.25rem; padding: 0.75rem 0; border-bottom: 1px solid rgba(255,255,255,0.1); }
     .empty { color: #94a3b8; }`
  ]
})
export class TicketsComponent implements OnInit {
  tickets: Ticket[] = [];
  form: FormGroup;
  TicketPriority = TicketPriority;

  constructor(private workspaceService: WorkspaceService, private fb: FormBuilder) {
    this.form = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      priority: [TicketPriority.Medium, Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadTickets();
  }

  loadTickets(): void {
    this.workspaceService.getTickets().subscribe(tickets => this.tickets = tickets);
  }

  submit(): void {
    if (this.form.invalid) return;
    this.workspaceService.createTicket(this.form.value).subscribe(() => {
      this.form.reset({ priority: TicketPriority.Medium });
      this.loadTickets();
    });
  }
}
