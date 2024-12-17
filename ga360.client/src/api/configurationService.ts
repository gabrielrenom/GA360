import { Country } from "types/country";

export const endpoints = {
  key: '/api/configuration',
  redirect: '/redirecturl',
  logout: '/logouturl',
};

export async function getRedirectUrl(): Promise<string> {
  const response = await fetch(endpoints.key + endpoints.redirect, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok getting the countries');
  }

  const result= await response.json();
  return result.redirectUrl;
}

export async function getLogoutUrl(): Promise<string> {
  const response = await fetch(endpoints.key + endpoints.logout, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok getting the countries');
  }

  const result= await response.json();
  return result.logoutUrl;
}