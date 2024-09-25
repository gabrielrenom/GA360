import { Ethnicity } from 'types/ethnicity';

export const endpoints = {
  key: 'api/ethnicity',
  list: '/list',
};

export async function getEthnicities(): Promise<Ethnicity[]> {
  const response = await fetch(endpoints.key + endpoints.list, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response for getEthnicities was not ok');
  }

  const data: Ethnicity[] = await response.json();
  return data;
}
