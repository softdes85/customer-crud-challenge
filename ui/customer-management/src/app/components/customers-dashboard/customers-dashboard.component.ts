import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import {
  MatPaginator,
  MatPaginatorModule,
  PageEvent,
} from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { CustomersService } from '../../services/customers-service.service';
import { Customer } from '../../models/customer';
import { HttpClientModule } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { CustomerCreateDialogComponent } from '../customer-create-dialog/customer-create-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { SelectionModel } from '@angular/cdk/collections';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { CustomerStateManagementService } from '../../services/customer-state-management.service';
import { CustomerViewComponent } from '../customer-view/customer-view.component';

@Component({
  selector: 'app-customers-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    MatTableModule,
    MatButtonModule,
    MatPaginatorModule,
    MatInputModule,
    MatIconModule,
    CustomerViewComponent,
  ],
  templateUrl: './customers-dashboard.component.html',
  styleUrl: './customers-dashboard.component.scss',
})
export class CustomersDashboardComponent implements OnInit, OnDestroy {
  displayedColumns1: string[] = ['position', 'name', 'weight', 'symbol'];
  private subscriptions: Subscription = new Subscription();
  displayedColumns: string[] = [
    'id',
    'firstName',
    'lastName',
    'email',
    'actions',
  ];

  pageSizeOptions: number[] = [5, 10, 25, 50];
  pageSize = 10;
  pageIndex = 0;
  dataSource: Customer[] = [];
  selection: Customer | null = new Customer();
  length = 0;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private customerService: CustomersService,
    private customerStateManagermentService: CustomerStateManagementService,
    public dialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.loadSelectedCustomer();
    this.subscriptions.add(
      this.customerStateManagermentService.selectedCustomer$.subscribe(
        (customer) => {
          this.saveSelectedCustomer(customer);
        }
      )
    );
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.route.queryParams.subscribe((params) => {
        this.pageSize = params['pageSize'] ? +params['pageSize'] : 10;
        this.pageIndex = params['pageIndex'] ? +params['pageIndex'] : 0;

        // Fetch data based on the updated pagination parameters
        this.loadCustomers();
      })
    );
  }
  ngAfterViewInit() {
    this.loadCustomers();
  }
  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onPageChange(event: any): void {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex;

    // Update the query parameters
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { pageSize: this.pageSize, pageIndex: this.pageIndex },
      queryParamsHandling: 'merge',
    });
  }

  loadSelectedCustomer() {
    const storedSelection = sessionStorage.getItem('selectedRow');
    if (storedSelection) {
      this.customerStateManagermentService.selectCustomer(
        JSON.parse(storedSelection)
      );
    }
  }
  saveSelectedCustomer(row: any): void {
    this.selection = row;
    if (this.selection) {
      sessionStorage.setItem('selectedRow', JSON.stringify(this.selection));
    } else {
      sessionStorage.removeItem('selectedRow');
    }
  }

  loadCustomers() {
    this.customerService
      .getAll(this.pageIndex + 1, this.pageSize)
      .subscribe((data) => {
        this.dataSource = data.items;
        this.length = data.totalCount;
      });
  }

  onRowClick(row: Customer): void {
    this.customerStateManagermentService.selectCustomer(row);
  }

  deleteCustomer(customer: Customer) {
    // Confirm before deletion
    if (confirm(`Are you sure you want to delete ${customer.firstName}?`)) {
      this.customerService.delete(customer.id).subscribe(() => {
        //
        this.customerStateManagermentService.clearSelectedCustomer();
        this.loadCustomers();
      });
    }
  }
  openDialog(customer?: Customer): void {
    const dialogRef = this.dialog.open(CustomerCreateDialogComponent, {
      width: '250px',
      data: { customer: customer || {}, isEditMode: !!customer },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (customer)
        this.customerStateManagermentService.selectCustomer(customer);
      else this.customerStateManagermentService.clearSelectedCustomer();
      this.loadCustomers();
    });
  }

  createCustomer() {
    this.openDialog();
  }

  editCustomer(customer: Customer) {
    console.log(customer);
    this.openDialog(customer);
  }
}
