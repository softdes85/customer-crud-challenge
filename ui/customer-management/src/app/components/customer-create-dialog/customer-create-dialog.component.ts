import { Component, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomersService } from '../../services/customers-service.service';
import { Customer } from '../../models/customer';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-customer-create-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatInputModule,
    ReactiveFormsModule
  ],
  templateUrl: './customer-create-dialog.component.html',
  styleUrl: './customer-create-dialog.component.scss'
})
export class CustomerCreateDialogComponent {
  customerForm: FormGroup;

  constructor(
    private customerService: CustomersService,
    public dialogRef: MatDialogRef<CustomerCreateDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder
  ) {

    //this.isEditMode = data && data.customer != null;
    // Initialize form
    this.customerForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]]
    });

    if (this.data.isEditMode) {
      this.customerForm.patchValue(data.customer);
    }
  }

  onSave() {
    if (this.customerForm.valid) {
      const customerData: Customer = this.customerForm.value;

      const observer = {
        next: (response: any) => {
          this.dialogRef.close(true);
        },
        error: (error: any) => {
          console.log(error);
        },
        complete: () => {
          console.log('Customer creation completed');
        }
      };

      if (this.data.isEditMode) {
        this.customerService.update(this.data.customer.id, customerData).subscribe(observer);
      } else {
        this.customerService.create(customerData).subscribe(observer);
      }
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
