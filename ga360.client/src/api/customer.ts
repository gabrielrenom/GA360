import useSWR, { mutate } from 'swr';
import { useMemo } from 'react';

// utils
import { fetcher } from 'utils/axios';

// types
import { BasicCustomer, CustomerList, CustomerListExtended, CustomerProps } from 'types/customer';
import { mapCustomerApiModelToCustomerList, mapCustomerListToCustomerApiModel, mapCustomerListToCustomerApiModelExtended } from 'types/customerApiModel';

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
  get: '/get'
};

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

//export function useGetCustomer() {
//    const [data, setData] = useState<any>(null);
//    const [isLoading, setIsLoading] = useState<boolean>(true);
//    const [error, setError] = useState<Error | null>(null);
//    const [isValidating, setIsValidating] = useState<boolean>(false);

//    useEffect(() => {
//        const fetchData = async () => {
//            setIsValidating(true);
//            try {
//                const result = await fetcher(endpoints.key + endpoints.list);
//                setData(result);
//            } catch (err) {
//                setError(err);
//            } finally {
//                setIsLoading(false);
//                setIsValidating(false);
//            }
//        };

//        fetchData();
//    }, []);

//    const memoizedValue = useMemo(
//        () => ({
//            customers: data?.customers as CustomerList[],
//            customersLoading: isLoading,
//            customersError: error,
//            customersValidating: isValidating,
//            customersEmpty: !isLoading && !data?.customers?.length
//        }),
//        [data, error, isLoading, isValidating]
//    );

//    return memoizedValue;
//}

export async function insertCustomer(newCustomer: CustomerListExtended) {
  // to update local state based on key
  // mutate(
  //   endpoints.key + endpoints.list,
  //   (currentCustomer: any) => {
  //     newCustomer.id = currentCustomer.customers.length + 1;
  //     const addedCustomer: CustomerList[] = [...currentCustomer.customers, newCustomer];

  //     return {
  //       ...currentCustomer,
  //       customers: addedCustomer
  //     };
  //   },
  //   false
  // );
  console.log("Insert New customer:",newCustomer);
  const mappedCustomer = mapCustomerListToCustomerApiModelExtended(newCustomer);
  console.log(mappedCustomer);
  const response = await fetch('/api/customer/create',{
    method: 'POST',
    headers: {
        'Content-Type': 'application/json'
    },
    body: JSON.stringify(mappedCustomer)
  });
  const data = response.json();


  // to hit server
  // you may need to refetch latest data after server hit and based on your logic
  //   const data = { newCustomer };
  //   await axios.post(endpoints.key + endpoints.insert, data);
}

// export async function updateCustomerWithDocuments(customerId: number, updatedCustomer: CustomerListExtended, documents: File[]) {

//   const mappedCustomer = mapCustomerListToCustomerApiModelExtended(updatedCustomer);

//   const formData = new FormData();
//   formData.append('Customer', JSON.stringify(mappedCustomer));
//   documents.forEach((file) => formData.append('Files', file));

//   //formData.append('Files', new File([''], '133694255036516995.jpg', { type: 'image/jpeg' }));

// const options: RequestInit = {
//   method: 'PUT',
//   headers: {
//     'accept': '*/*',
//     // 'Content-Type' should not be set when sending FormData
//   },
//   body: formData,
// };

// // fetch('/api/customer/updatewithdocuments/'+customerId, options)
// //   .then(response => response.json())
// //   .then(data => console.log(data))
// //   .catch(error => console.error('Error:', error));
 
// }

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
  const mappedCustomer = mapCustomerListToCustomerApiModelExtended(newCustomer);

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
