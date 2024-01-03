import { Component, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { CustomersService } from '../../services/customers-service.service';
import { Customer } from '../../models/customer';
import { HttpClientModule } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { CustomerCreateDialogComponent } from '../customer-create-dialog/customer-create-dialog.component';

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
  dataSource: Customer[] = [];

  length = 0;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private customerService: CustomersService, public dialog: MatDialog) {

  }

  ngAfterViewInit() {
    this.loadCustomers();
    //this.dataSource.paginator = this.paginator;
  }

  loadCustomers() {
    const page = this.paginator ? this.paginator.pageIndex : 0;
    const pageSize = this.paginator ? this.paginator.pageSize : 10;

    this.customerService.getAll(page, pageSize)
      .subscribe(data => {
        this.dataSource = data.items;
        this.length = data.totalCount;
      });
  }



  deleteCustomer(customer: Customer) {
    // Confirm before deletion
    if (confirm(`Are you sure you want to delete ${customer.firstName}?`)) {
      this.customerService.delete(customer.id).subscribe(() => {
        this.loadCustomers();
      });
    }
  }
  openDialog(customer?: Customer): void {
    const dialogRef = this.dialog.open(CustomerCreateDialogComponent, {
      width: '250px',
      data: { customer: customer || {}, isEditMode: !!customer }
    });

    dialogRef.afterClosed().subscribe(result => {
      // Handle the result, refresh data if needed
    });
  }

  createCustomer() {
    this.openDialog();
  }

  editCustomer(customer: Customer) {
    this.openDialog(customer);
  }
  
}
