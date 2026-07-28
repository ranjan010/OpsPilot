import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';

describe('AuthService', () => {
  let service: AuthService;

  beforeEach(() => {
    localStorage.clear();
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthService);
  });

  it('should authenticate with the mocked demo credentials', () => {
    expect(service.login('demo@ai.dev', 'password123')).toBeTrue();
    expect(service.isAuthenticated).toBeTrue();
  });

  it('should reject invalid credentials', () => {
    expect(service.login('wrong@example.com', 'secret')).toBeFalse();
    expect(service.isAuthenticated).toBeFalse();
  });
});
