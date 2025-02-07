
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
  industriesstatsbygrainingid: '/api/dashboard/industriesstatsbytrainingcentreid',

};
export interface IndustryPercentage {
  industry: string;
  percentage: number;
}


export async function getDashboardStats(trainingCentreId?: number): Promise<DashboardStats> {
  const response = await fetch(trainingCentreId===null?endpoints.learnerstats:endpoints.learnerstats+"/"+trainingCentreId , {
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


export async function getIndustriesStatsByTrainingCentreId(trainingCentreId: number): Promise<IndustryPercentage[]> {
  const response = await fetch(`${endpoints.industriesstatsbygrainingid}/${trainingCentreId}`, {
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