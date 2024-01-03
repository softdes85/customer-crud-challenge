import { RouterModule, Routes } from '@angular/router';
import { CustomersDashboardComponent } from './components/customers-dashboard/customers-dashboard.component';
import { NgModule } from '@angular/core';


export const routes: Routes = [    
    { path: 'customers', component: CustomersDashboardComponent },
    { path: 'customers/:id', component: CustomersDashboardComponent },
];

