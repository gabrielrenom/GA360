import { Skill } from 'types/skill';

export const endpoints = {
  key: 'api/skill',
  list: '/list',
};

export async function getSkills(): Promise<Skill[]> {
  const response = await fetch(endpoints.key + endpoints.list, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok getting the skills');
  }

  const data: Skill[] = await response.json();
  return data;
}