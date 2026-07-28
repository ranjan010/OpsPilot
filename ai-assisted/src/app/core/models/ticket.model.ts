export enum TicketStatus {
  Open = 0,
  InProgress = 1,
  Review = 2,
  Resolved = 3,
  Closed = 4
}

export enum TicketPriority {
  Low = 0,
  Medium = 1,
  High = 2,
  Urgent = 3
}

export interface Ticket {
  id: string;
  key: string;
  title: string;
  description: string;
  status: TicketStatus;
  priority: TicketPriority;
  assigneeName?: string | null;
  dueDate?: string | null;
  createdAt: string;
}

export interface CreateTicketRequest {
  title: string;
  description: string;
  priority: TicketPriority;
  projectId?: string;
  assigneeId?: string;
  dueDate?: string;
}
