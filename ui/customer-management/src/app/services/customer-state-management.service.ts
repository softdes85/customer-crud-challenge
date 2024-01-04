import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Customer } from '../models/customer';

@Injectable({
  providedIn: 'root',
})
export class CustomerStateManagementService {
  private selectedCustomerSubject = new BehaviorSubject<Customer | null>(null);
  selectedCustomer$ = this.selectedCustomerSubject.asObservable();

  constructor() {}

  selectCustomer(customer: Customer): void {
    this.selectedCustomerSubject.next(customer);
  }

  clearSelectedCustomer(): void {
    this.selectedCustomerSubject.next(null);
  }
}
