import { Component, OnDestroy } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { CustomerStateManagementService } from '../../services/customer-state-management.service';
import { Subscription } from 'rxjs';
import { Customer } from '../../models/customer';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-customer-view',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule],
  templateUrl: './customer-view.component.html',
  styleUrl: './customer-view.component.scss',
})
export class CustomerViewComponent implements OnDestroy {
  customer: Customer | null = null;
  private subscriptions: Subscription = new Subscription();
  constructor(
    private customerStateManagermentService: CustomerStateManagementService
  ) {
    this.subscriptions.add(
      this.customerStateManagermentService.selectedCustomer$.subscribe(
        (customer) => {
          this.customer = customer;
        }
      )
    );
  }
  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
