export interface Qualification {
    id: number;
    name: string;
    registrationDate: Date;
    expectedDate: Date;
    certificateDate: Date;
    certificateNumber: number;
    status: number;
  }

export const endpoints = {
  key: '/api/qualification'
};

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
