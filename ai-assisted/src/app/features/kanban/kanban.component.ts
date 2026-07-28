import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkspaceService } from '../../core/services/workspace.service';
import { Ticket, TicketStatus } from '../../core/models/ticket.model';

export interface KanbanColumn {
  status: TicketStatus;
  label: string;
  colorClass: string;
  tickets: Ticket[];
}

@Component({
  standalone: true,
  selector: 'app-kanban',
  imports: [CommonModule],
  templateUrl: './kanban.component.html',
  styleUrls: ['./kanban.component.scss']
})
export class KanbanComponent implements OnInit {
  columns: KanbanColumn[] = [
    { status: TicketStatus.Open,       label: 'Open',        colorClass: 'col-open',       tickets: [] },
    { status: TicketStatus.InProgress, label: 'In Progress', colorClass: 'col-inprogress', tickets: [] },
    { status: TicketStatus.Review,     label: 'Review',      colorClass: 'col-review',     tickets: [] },
    { status: TicketStatus.Resolved,   label: 'Resolved',    colorClass: 'col-resolved',   tickets: [] },
    { status: TicketStatus.Closed,     label: 'Closed',      colorClass: 'col-closed',     tickets: [] },
  ];

  isLoading = true;
  errorMessage = '';
  draggingTicket: Ticket | null = null;
  dragOverColumn: TicketStatus | null = null;

  constructor(private workspaceService: WorkspaceService) {}

  ngOnInit(): void {
    this.loadTickets();
  }

  loadTickets(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.workspaceService.getTickets().subscribe({
      next: (tickets) => {
        this.columns.forEach(col => col.tickets = []);
        tickets.forEach(t => {
          const col = this.columns.find(c => c.status === t.status);
          if (col) col.tickets.push(t);
        });
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Failed to load tickets.';
        this.isLoading = false;
      }
    });
  }

  onDragStart(ticket: Ticket): void {
    this.draggingTicket = ticket;
  }

  onDragOver(event: DragEvent, colStatus: TicketStatus): void {
    event.preventDefault();
    this.dragOverColumn = colStatus;
  }

  onDragLeave(): void {
    this.dragOverColumn = null;
  }

  onDrop(event: DragEvent, targetStatus: TicketStatus): void {
    event.preventDefault();
    this.dragOverColumn = null;
    if (!this.draggingTicket || this.draggingTicket.status === targetStatus) {
      this.draggingTicket = null;
      return;
    }

    const ticket = this.draggingTicket;
    this.draggingTicket = null;

    // Optimistic UI update
    const fromCol = this.columns.find(c => c.status === ticket.status);
    const toCol   = this.columns.find(c => c.status === targetStatus);
    if (fromCol) fromCol.tickets = fromCol.tickets.filter(t => t.id !== ticket.id);
    if (toCol)   toCol.tickets = [{ ...ticket, status: targetStatus }, ...toCol.tickets];

    this.workspaceService.updateTicketStatus(ticket.id, targetStatus).subscribe({
      error: () => {
        // Roll back on error
        if (toCol)   toCol.tickets = toCol.tickets.filter(t => t.id !== ticket.id);
        if (fromCol) fromCol.tickets = [ticket, ...fromCol.tickets];
        this.errorMessage = 'Failed to update ticket status. Please try again.';
      }
    });
  }

  getPriorityLabel(priority: number): string {
    const labels: Record<number, string> = { 0: 'Low', 1: 'Medium', 2: 'High', 3: 'Urgent' };
    return labels[priority] ?? 'Medium';
  }

  getPriorityClass(priority: number): string {
    const classes: Record<number, string> = { 0: 'prio-low', 1: 'prio-medium', 2: 'prio-high', 3: 'prio-urgent' };
    return classes[priority] ?? 'prio-medium';
  }

  trackByTicketId(_: number, ticket: Ticket): string {
    return ticket.id;
  }

  trackByColStatus(_: number, col: KanbanColumn): number {
    return col.status;
  }
}
