import { ComponentFixture, TestBed } from '@angular/core/testing';
import { KanbanComponent } from './kanban.component';
import { WorkspaceService } from '../../core/services/workspace.service';
import { of, throwError } from 'rxjs';
import { TicketStatus, TicketPriority } from '../../core/models/ticket.model';

describe('KanbanComponent', () => {
  let component: KanbanComponent;
  let fixture: ComponentFixture<KanbanComponent>;
  let workspaceServiceSpy: jasmine.SpyObj<WorkspaceService>;

  const mockTickets = [
    {
      id: 'ticket-1',
      key: 'OPS-001',
      title: 'Test Ticket',
      description: 'A test ticket',
      status: TicketStatus.Open,
      priority: TicketPriority.Medium,
      assigneeName: null,
      dueDate: null,
      createdAt: new Date().toISOString()
    }
  ];

  beforeEach(async () => {
    workspaceServiceSpy = jasmine.createSpyObj('WorkspaceService', ['getTickets', 'updateTicketStatus']);
    workspaceServiceSpy.getTickets.and.returnValue(of(mockTickets));

    await TestBed.configureTestingModule({
      declarations: [KanbanComponent],
      providers: [{ provide: WorkspaceService, useValue: workspaceServiceSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(KanbanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have 5 columns', () => {
    expect(component.columns.length).toBe(5);
  });

  it('should load tickets into the correct column', () => {
    const openCol = component.columns.find(c => c.status === TicketStatus.Open);
    expect(openCol?.tickets.length).toBe(1);
    expect(openCol?.tickets[0].key).toBe('OPS-001');
  });

  it('should show error message on load failure', () => {
    workspaceServiceSpy.getTickets.and.returnValue(throwError(() => new Error('Network error')));
    component.loadTickets();
    expect(component.errorMessage).toBeTruthy();
  });

  it('should return correct priority label', () => {
    expect(component.getPriorityLabel(0)).toBe('Low');
    expect(component.getPriorityLabel(3)).toBe('Urgent');
  });
});
