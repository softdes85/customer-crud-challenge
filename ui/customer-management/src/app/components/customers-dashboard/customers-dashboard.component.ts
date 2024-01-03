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
  dataSource = new MatTableDataSource<Customer>();
  pageIndex:number;
  pageSize:number;
  length:number;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private customerService: CustomersService, public dialog: MatDialog) {
    this.pageIndex = 0;
    this.pageSize = 10;
    this.length = 0;
  }

  ngAfterViewInit() {
    this.loadCustomers();
    this.dataSource.paginator = this.paginator;
  }

  loadCustomers(pageIndex: number = 0, pageSize: number = 10) {
    this.customerService.getAll(pageIndex, pageSize)
      .subscribe(data => {
        this.dataSource.data = data.items;
        
        this.pageIndex = data.currentPage;
        this.pageSize = data.pageSize;
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
  onPageChange(event: any) {   
    this.loadCustomers(event.pageIndex, event.pageSize);
  }
}
