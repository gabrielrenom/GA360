export interface CustomersWithCourseQualificationRecordsViewModel {
  id: number;
  customerId: number;
  email: string;
  courseId?: number;
  courseName: string;
  qualificationId?: number;
  qualificationName: string;
  certificateId?: number;
  certificateName: string;
  trainingCentreId?: number;
  trainingCentre: string;
  progression: number;
  qualificationStatusId?: number;
  qualificationStatus: string;
}

export interface Qualification {
    id: number;
    name: string;
    registrationDate: Date;
    expectedDate: Date;
    certificateDate: Date;
    certificateNumber: number;
    status: number;
  }

export interface QualificationStatus {
    id: number;
    name: string;
    description: string;
    qualificationCustomerCourseCertificates: any; // Adjust type as needed
    isDeleted: boolean;
    deleterUserId: number | null;
    deletionTime: string | null;
    createdBy: string;
    modifiedBy: string;
    createdAt: string;
    modifiedAt: string;
    tenantId: number | null;
  }
  
export const endpoints = {
  key: '/api/qualification',
  fullrecord: '/api/customer/customerswithcoursequalificationrecords',
  qualificationstatuses: '/api/qualification/qualificationstatuses',
};

export async function getQualificationStatuses(): Promise<QualificationStatus[]> {
  const response = await fetch(endpoints.qualificationstatuses, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data: QualificationStatus[] = await response.json();
  return data;
}

export async function getQualifications(): Promise<Qualification[]> {
  const response = await fetch(endpoints.key, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data: Qualification[] = await response.json();
  return data;
}

export async function getQualification(id: number): Promise<Qualification> {
  const response = await fetch(`${endpoints.key}/${id}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data: Qualification = await response.json();
  return data;
}

export async function addQualification(qualification: Qualification): Promise<Qualification> {
  console.log("MY QUALI", qualification)
  const response = await fetch(endpoints.key, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
    body: JSON.stringify(qualification),
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const createdQualification: Qualification = await response.json(); 
  return createdQualification;
}

export async function updateQualification(id: number, qualification: Qualification): Promise<void> {
  const response = await fetch(`${endpoints.key}/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
    body: JSON.stringify(qualification),
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
}

export async function deleteQualification(id: number): Promise<void> {
  const response = await fetch(`${endpoints.key}/${id}`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
}

export async function getAllCustomersWithCourseQualificationRecords(pageNumber?: number, pageSize?: number, orderBy: string = "Email", ascending: boolean = true): Promise<CustomersWithCourseQualificationRecordsViewModel[]> {
  pageNumber = 1
  pageSize = 20
  const response = await fetch(`${endpoints.fullrecord}?pageNumber=${pageNumber}&pageSize=${pageSize}&orderBy=${orderBy}&ascending=${ascending}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data: CustomersWithCourseQualificationRecordsViewModel[] = await response.json();
  return data;
}

export async function deleteCustomersWithCourseQualificationRecords(id: number): Promise<void> {
  const response = await fetch(`${endpoints.fullrecord}/${id}`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
}

export async function updateCustomersWithCourseQualificationRecords(id: number, customer: CustomersWithCourseQualificationRecordsViewModel): Promise<CustomersWithCourseQualificationRecordsViewModel> {
  const response = await fetch(`${endpoints.fullrecord}/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
    body: JSON.stringify(customer),
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  const data: CustomersWithCourseQualificationRecordsViewModel = await response.json();
  return data;
}

export async function createCustomersWithCourseQualificationRecords(customer: CustomersWithCourseQualificationRecordsViewModel): Promise<CustomersWithCourseQualificationRecordsViewModel> {
  const response = await fetch(endpoints.fullrecord, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
    body: JSON.stringify(customer),
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  const data: CustomersWithCourseQualificationRecordsViewModel = await response.json();
  return data;
}