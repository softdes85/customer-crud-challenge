import { TestBed } from '@angular/core/testing';

import { CustomerStateManagementService } from './customer-state-management.service';

describe('CustomerStateManagementService', () => {
  let service: CustomerStateManagementService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CustomerStateManagementService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
