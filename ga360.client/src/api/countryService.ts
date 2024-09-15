import { Country } from "types/country";

export const endpoints = {
  key: 'api/country',
  list: '/list',
};

export async function getCountries(): Promise<Country[]> {
  const response = await fetch(endpoints.key + endpoints.list, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok getting the countries');
  }

  const data: Country[] = await response.json();
  return data;
}
