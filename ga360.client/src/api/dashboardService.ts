
export interface DashboardStats {
    stats: DashboardStat[]
  }

export interface DashboardStat {
  statisticType:number;
  percentage:number;
  totalYear:number;
  total:number;
}
  
export const endpoints = {
  key: '/api/dashboard',
  learnerstats: '/api/dashboard/learnersstats',
  industriesstats: '/api/dashboard/industriesstats',

};
export interface IndustryPercentage {
  industry: string;
  percentage: number;
}


export async function getDashboardStats(): Promise<DashboardStats> {
  const response = await fetch(endpoints.learnerstats , {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data: DashboardStats = await response.json();

  return data;
}

export async function getIndustriesStats(): Promise<IndustryPercentage[]> {
  const response = await fetch(endpoints.industriesstats , {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data: IndustryPercentage[] = await response.json();

  return data;
}
