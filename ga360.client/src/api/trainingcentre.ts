import { TrainingCentre } from 'types/trainingcentretypes';

export const endpoints = {
  key: '/api/trainingcentre',
  list: '/list',
};



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
