// import { TrainingCentre } from 'types/trainingcentretypes';

export interface Address {
  id: number;
  street: string;
  number: string;
  postcode: string;
  city: string;
}

export interface TrainingCentre {
  id: number;
  name: string;
  addressId: number;
  address: Address;
}

export interface TrainingCentreWithAddress {
  id: number;
  name: string;
  addressId: number;
  street: string;
  number: string;
  postcode: string;
  city: string;
}


export const endpoints = {
  key: '/api/trainingcentre',
  list: '/list',
  gettrainingcentresbyqualificationid: '/api/trainingcentre/qualification',
};

export async function getTrainingCentresByQualificationId(id:number): Promise<TrainingCentre[]> {
  const response = await fetch(`${endpoints.gettrainingcentresbyqualificationid}/${id}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data: TrainingCentre[] = await response.json();
  return data;
}


export async function getTrainingCentres(): Promise<TrainingCentre[]> {
  const response = await fetch(endpoints.key + endpoints.list, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data: TrainingCentre[] = await response.json();
  return data;
}

export async function getTrainingCentre(id: number): Promise<TrainingCentre> {
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

  const data: TrainingCentre = await response.json();
  return data;
}

export async function addTrainingCentre(trainingCentre: TrainingCentreWithAddress): Promise<TrainingCentreWithAddress> {
  const response = await fetch(endpoints.key, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
    body: JSON.stringify(trainingCentre),
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const trainingCentreResult: TrainingCentreWithAddress = await response.json()
  return trainingCentreResult;
}

export async function updateTrainingCentre(id: number, trainingCentre: TrainingCentreWithAddress): Promise<TrainingCentreWithAddress> {
  const response = await fetch(`${endpoints.key}/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
    body: JSON.stringify(trainingCentre),
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  const trainingCentreResult: TrainingCentreWithAddress = await response.json()
  return trainingCentreResult;
}

export async function deleteTrainingCentre(id: number): Promise<void> {
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
