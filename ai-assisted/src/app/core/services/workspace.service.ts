import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateProjectRequest, Project } from '../models/project.model';
import { CreateTicketRequest, Ticket, TicketStatus } from '../models/ticket.model';
import { Member } from '../models/member.model';

@Injectable({ providedIn: 'root' })
export class WorkspaceService {
  private readonly baseUrl = 'https://localhost:58958/api';

  constructor(private http: HttpClient) {}

  getProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(`${this.baseUrl}/projects`);
  }

  createProject(request: CreateProjectRequest): Observable<Project> {
    return this.http.post<Project>(`${this.baseUrl}/projects`, request);
  }

  getTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(`${this.baseUrl}/tickets`);
  }

  createTicket(request: CreateTicketRequest): Observable<Ticket> {
    return this.http.post<Ticket>(`${this.baseUrl}/tickets`, request);
  }

  updateTicketStatus(ticketId: string, status: TicketStatus): Observable<Ticket> {
    return this.http.patch<Ticket>(`${this.baseUrl}/tickets/${ticketId}/status`, { status });
  }

  getMembers(): Observable<Member[]> {
    return this.http.get<Member[]>(`${this.baseUrl}/organizations/members`);
  }
}
