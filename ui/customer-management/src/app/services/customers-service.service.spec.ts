import { CustomersService } from './customers-service.service';
import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { environment } from '../../environments/environment';


describe('CustomersServiceService', () => {
  let service: CustomersService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CustomersService]
    });
    service = TestBed.inject(CustomersService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); // Verify that no unmatched requests are outstanding.
  });

  it('should retrieve customers with pagination', () => {
    const mockResponse = {
      items: [
        { id: 1, firstName: 'John', lastName: 'Doe' },
        { id: 2, firstName: 'Jane', lastName: 'Doe' }
      ],
      currentPage: 1,
      totalPages: 5,
      pageSize: 2,
      totalCount: 10
    };
    const pageNumber = 1;
    const pageSize = 2;
  
    service.getAll(pageNumber, pageSize).subscribe(response => {
      expect(response.items.length).toBe(2);
      expect(response.currentPage).toBe(1);
      expect(response.totalPages).toBe(5);
      expect(response.pageSize).toBe(2);
      expect(response.totalCount).toBe(10);
    });
  
    const request = httpMock.expectOne(req => 
      req.url === `${environment.apiUrl}/customers` &&
      req.params.get('pageNumber') === '1' &&
      req.params.get('pageSize') === '2'
    );
    expect(request.request.method).toBe('GET');
    request.flush(mockResponse);
  });

  it('should create a customer', () => {
    const newCustomer = { id: 0, firstName: 'Alice', lastName: 'Smith', email: 'mail@gmail.com' };
  
    service.create(newCustomer).subscribe(customer => {
      expect(customer.firstName).toBe('Alice');
      expect(customer.lastName).toBe('Smith');
    });
  
    const request = httpMock.expectOne(`${environment.apiUrl}/customers`);
    expect(request.request.method).toBe('POST');
    request.flush({ ...newCustomer });
  });

  it('should update a customer', () => {
    const updatedCustomer = { id: 1, firstName: 'John', lastName: 'Smith', email: 'mail@gmail.com' };
  
    service.update(1, updatedCustomer).subscribe(customer => {
      expect(customer).toEqual(updatedCustomer);
    });
  
    const request = httpMock.expectOne(`${environment.apiUrl}/customers/1`);
    expect(request.request.method).toBe('PUT');
    request.flush(updatedCustomer);
  });

  it('should delete a customer', () => {
    service.delete(1).subscribe(response => {
      expect(response).toBeNull();
    });
  
    const request = httpMock.expectOne(`${environment.apiUrl}/customers/1`);
    expect(request.request.method).toBe('DELETE');
    request.flush(null);
  });
});