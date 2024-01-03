import { Component, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { CustomersService } from '../../services/customers-service.service';
import { Customer } from '../../models/customer';
import { HttpClientModule } from '@angular/common/http';

interface FilterValues {
  firstName: string;
  lastName: string;
  email: string;
}

@Component({
  selector: 'app-customers-dashboard',
  standalone: true,
  imports: [HttpClientModule, MatTableModule, MatPaginatorModule, MatInputModule],
  templateUrl: './customers-dashboard.component.html',
  styleUrl: './customers-dashboard.component.scss'
})
export class CustomersDashboardComponent {
  displayedColumns: string[] = ['id', 'firstName', 'lastName', 'email', 'actions'];
  dataSource = new MatTableDataSource<Customer>();
  filterValues: FilterValues = { firstName: '', lastName: '', email: '' };

  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;

  constructor(private customerService: CustomersService) {
    this.loadCustomers();
  }

  ngAfterViewInit() {

    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
    }
  }

  loadCustomers() {
    this.customerService.getAll() 
      .subscribe(data => this.dataSource.data = data.items);
  }

  applyFilter(event: Event, filterKey: keyof FilterValues) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterValues[filterKey] = filterValue.trim().toLowerCase();
    this.dataSource.filter = JSON.stringify(this.filterValues);
  }
  viewCustomer(customer: Customer) {
    // Handle the view action (e.g., open a dialog or navigate to a detail view)
  }

  editCustomer(customer: Customer) {
    // Handle the edit action (e.g., open an edit form in a dialog)
  }

  deleteCustomer(customer: Customer) {
    // Confirm before deletion
    if (confirm(`Are you sure you want to delete ${customer.firstName}?`)) {
      this.customerService.delete(customer.id).subscribe(() => {
        this.loadCustomers();
      });
    }
  }
}
