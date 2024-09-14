// types
import { TrainingCentre } from 'types/trainingcentre';

export const endpoints = {
  key: 'api/trainingcentre',
  list: '/list', // server URL
};

export async function getTrainingCentres() {
  const response = await fetch(endpoints.key + endpoints.list, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  const result = response.json()
  console.log("TRAINING",result)

  if (!response.ok) {
    throw new Error('getTrainingCentres response was not ok');
  }

  return result;
}
