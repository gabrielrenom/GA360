import useSWR, { mutate } from 'swr';
import { useMemo } from 'react';

export interface CustomerBatchUploadResponse { customers: CustomerUploadResponse[]; }
export interface CustomerUpload {
  id: number;
  FirstName: string;
  LastName: string;
  Name: string;
  Gender: string;
  Age: number;
  Contact: string;
  Email: string;
  Country: string;
  Location: string;
  FatherName: string;
  Role: string;
  About: string;
  Status: number;
  Time: string;
  Date: string;
  CountryName: string;
  Portfolio: string;
  DOB: string;
  Street: string;
  City: string;
  Number: string;
  Postcode: string;
  DateOfBirth: string;
  Ethnicity: string;
  Disability: string;
  EmployeeStatus: string;
  Employer: string;
  TrainingCentre: string;
  NationalInsurance: string;
}

export interface CustomerUploadResponse {
  id: number;
  firstName: string;
  lastName: string;
  name: string;
  gender: string;
  age: number;
  contact: string;
  email: string;
  country: string;
  location: string;
  fatherName: string;
  role: string;
  about: string;
  status: number;
  time: string;
  date: string;
  countryName: string;
  portfolio: string;
  dob: string;
  street: string;
  city: string;
  number: number;
  postcode: string;
  dateOfBirth: string;
  ethnicity: string;
  disability: string;
  employeeStatus: string;
  employer: string;
  trainingCentre: string;
  nationalInsurance: string;
}

export interface CustomerProfileModel {
  id: number;
  firstName: string;
  lastName: string;
  contact: string;
  about: string;
  gender: string;
  email: string;
  location: string;
  country: string;
  tenantId?: string; // Using string because JavaScript doesn't have native GUID support, usually stored as a string
  dob: string;
  ni: string;
  employmentStatus: string;
  employer: string;
  avatarImage: string;
  street: string;
  city: string;
  number: string;
  postcode: string;
  avgQualificationProgression: number;
}



// utils
import { fetcher } from 'utils/axios';

// types
import { BasicCustomer, CustomerList, CustomerListExtended, CustomerProps, User } from 'types/customer';
import { DocumentFileModel, mapCustomerApiModelToCustomerList, mapCustomerListToCustomerApiModel, mapCustomerListToCustomerApiModelExtended } from 'types/customerApiModel';

const initialState: CustomerProps = {
  modal: false
};

export const endpoints = {
  key: 'api/customer',
  list: '/list', // server URL
  modal: '/modal', // server URL
  insert: '/insert', // server URL
  update: '/update', // server URL
  delete: '/delete', // server URL
  get: '/get',
  user:'/user',
  batchupload:'/api/customer/batchupload',
  getdocuments: '/api/customer/get/documents',
  getcustomerprofilehighperformance: '/api/customer/get/profile',
};

export async function getDocumentsByUser(email: string): Promise<DocumentFileModel[]> {
  try {
    const response = await fetch(`${endpoints.getdocuments}/${email}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'X-CSRF': 'Dog',
      },
    });

    if (!response.ok) {
      throw new Error(`Network response was not ok: ${response.statusText}`);
    }

    return await response.json();
  } catch (error) {
    console.error('Failed to fetch documents:', error);
    throw error;
  }
}

export async function batchUploadCustomers(customers: CustomerUpload[]): Promise<CustomerBatchUploadResponse> {

  const response = await fetch(endpoints.batchupload, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      "X-CSRF": "Dog",
    },
    body: JSON.stringify({ customers }),
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  var data = response.json();

  return data;
}


export async function getUser(): Promise<User> {
  const response = await fetch("/api/customer/user", {
      headers: {
          "X-CSRF": "Dog",
      },
  });

  if (!response.ok) {
      throw new Error('Network response was not ok');
  }

  const result: User = await response.json();
  return result;
}

export async function getUserById(id: number): Promise<CustomerListExtended> {
  const response = await fetch(`/api/customer/get/full/${id}`, {
      headers: {
          "X-CSRF": "Dog",
      },
  });

  if (!response.ok) {
      throw new Error('Network response was not ok');
  }

  const result: CustomerListExtended = await response.json();
  return result;
}

export function useGetCustomer() {
  const { data, isLoading, error, isValidating } = useSWR(endpoints.key + endpoints.list, fetcher, {
    revalidateIfStale: false,
    revalidateOnFocus: false,
    revalidateOnReconnect: false
  });

  const memoizedValue = useMemo(
    () => ({
      customers: data?.customers as CustomerList[],
      customersLoading: isLoading,
      customersError: error,
      customersValidating: isValidating,
      customersEmpty: !isLoading && !data?.customers?.length
    }),
    [data, error, isLoading, isValidating]
  );

  return memoizedValue;
}

export async function insertCustomer(newCustomer: CustomerListExtended) {
  const mappedCustomer = mapCustomerListToCustomerApiModelExtended(newCustomer);
  const response = await fetch('/api/customer/create',{
    method: 'POST',
    headers: {
        'Content-Type': 'application/json'
    },
    body: JSON.stringify(mappedCustomer)
  });
  const data = response.json();
}

export async function getCustomerProfileHighPerformance(id: number): Promise<CustomerProfileModel> {
  try {
      const response = await fetch(`${endpoints.getcustomerprofilehighperformance}/${id}`, {
          headers: {
              "X-CSRF": "Dog",
          },
      });

      if (!response.ok) {
          throw new Error('Network response was not ok');
      }

      const result: CustomerProfileModel = await response.json();
      return result;

  } catch (error) {
      console.error('Error fetching customer profile:', error);
      throw error;
  }
}


export async function getBasicCandidates(): Promise<BasicCustomer[]> {
  const response = await fetch("/api/customer/list/basic", {
      headers: {
          "X-CSRF": "Dog",
      },
  });

  if (!response.ok) {
      throw new Error('Network response was not ok');
  }

  const result: BasicCustomer[] = await response.json();
  return result;
}

export async function getCandidate() {

  const response = await fetch("/api/customer/get", {
      headers: {
          "X-CSRF": "Dog",
      },
  });
  const result = await response.json();
  
  return result;
}

export async function getBasicCandidate() {

  const response = await fetch("/api/customer/get/basic", {
      headers: {
          "X-CSRF": "Dog",
      },
  });
  const result = await response.json();
  
  return result;
}

export async function insertCustomerWithDocuments(newCustomer: CustomerListExtended, documents: File[]): Promise<boolean> {
  console.log("MYNEWINSERT", newCustomer)
  const mappedCustomer = mapCustomerListToCustomerApiModelExtended(newCustomer);
  console.log("MYNEWINSERT 2", mappedCustomer)
  const formData = new FormData();
  formData.append('Customer', JSON.stringify(mappedCustomer));

  console.log("FILES",documents)

  if (documents && documents.length > 0) {
    documents.forEach((file) => formData.append('Files', file));
  }
  else
  {
    formData.append('Files', JSON.stringify([]));
  }

  const options: RequestInit = {
    method: 'POST',
    headers: {
      'accept': '*/*',
    },
    body: formData,
  };

  try {
    const response = await fetch(`/api/customer/create`, options);
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    const data = await response.json();
    console.log(data);
    return true; // Return true if the operation was successful
  } catch (error) {
    console.error('Error:', error);
    return false; // Return false if there was an error
  }

  return true;
}

export async function updateCustomerWithDocuments(customerId: number, updatedCustomer: CustomerListExtended, documents: File[]): Promise<boolean> {
  const mappedCustomer = mapCustomerListToCustomerApiModelExtended(updatedCustomer);

  const formData = new FormData();
  formData.append('Customer', JSON.stringify(mappedCustomer));

  console.log("FIKELS", documents)
  if (documents && documents.length > 0) {
    documents.forEach((file) => formData.append('Files', file));
  }
  else
  {
    formData.append('Files', JSON.stringify([]));
  }

  const options: RequestInit = {
    method: 'PUT',
    headers: {
      'accept': '*/*',
      // 'Content-Type' should not be set when sending FormData
    },
    body: formData,
  };

  try {
    const response = await fetch(`/api/customer/updatewithdocuments/${customerId}`, options);
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    const data = await response.json();
    console.log(data);
    return true; // Return true if the operation was successful
  } catch (error) {
    console.error('Error:', error);
    return false; // Return false if there was an error
  }

  return true;
}

  // try {
  //   const response = await fetch(`/api/customer/updatewithdocuments/${customerId}`, options);
  //   if (!response.ok) {
  //     throw new Error('Network response was not ok');
  //   }
  //   const data = await response.json();
  //   console.log(data);
  // } catch (error) {
  //   console.error('Error:', error);
  //   throw error; // Re-throw the error to be caught in the calling function
  // }


export async function updateCustomer(customerId: number, updatedCustomer: CustomerListExtended) {
  const mappedCustomer = mapCustomerListToCustomerApiModelExtended(updatedCustomer);
  console.log(customerId, "UPDATE", mappedCustomer);

  console.log(mappedCustomer);
  mappedCustomer.orderStatus = ""
  const response = await fetch('/api/customer/update/'+customerId,{
    method: 'PUT',
    headers: {
        'Content-Type': 'application/json'
    },
    body: JSON.stringify(mappedCustomer)
  });
  const data = response.json();
}

export async function deleteCustomer(customerId: number) {
  const response = await fetch('/api/customer/delete/'+customerId,{
    method: 'DELETE',
    headers: {
        'Content-Type': 'application/json'
    },
  });

  // to update local state based on key
  // mutate(
  //   endpoints.key + endpoints.list,
  //   (currentCustomer: any) => {
  //     const nonDeletedCustomer = currentCustomer.customers.filter((customer: CustomerList) => customer.id !== customerId);

  //     return {
  //       ...currentCustomer,
  //       customers: nonDeletedCustomer
  //     };
  //   },
  //   false
  // );

  // to hit server
  // you may need to refetch latest data after server hit and based on your logic
  //   const data = { customerId };
  //   await axios.post(endpoints.key + endpoints.delete, data);
}

export function useGetCustomerMaster() {
  const { data, isLoading } = useSWR(endpoints.key + endpoints.modal, () => initialState, {
    revalidateIfStale: false,
    revalidateOnFocus: false,
    revalidateOnReconnect: false
  });

  const memoizedValue = useMemo(
    () => ({
      customerMaster: data,
      customerMasterLoading: isLoading
    }),
    [data, isLoading]
  );

  return memoizedValue;
}

export function handlerCustomerDialog(modal: boolean) {
  // to update local state based on key

  mutate(
    endpoints.key + endpoints.modal,
    (currentCustomermaster: any) => {
      return { ...currentCustomermaster, modal };
    },
    false
  );
}

function useState<T>(arg0: boolean): [any, any] {
    throw new Error('Function not implemented.');
}

function useEffect(arg0: () => void, arg1: never[]) {
    throw new Error('Function not implemented.');
}
