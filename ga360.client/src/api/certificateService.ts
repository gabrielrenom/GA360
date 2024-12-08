export interface Certificate {
    id: number;
    name: string;
    charge: string;
    type: string;
    qualificationCustomerCourseCertificates: any; // Adjust type as needed
    documentCertificates: any; // Adjust type as needed
    isDeleted: boolean;
    deleterUserId: number | null;
    deletionTime: string | null;
    createdBy: string;
    modifiedBy: string;
    createdAt: string; // You might want to use Date if the time will be managed as Date type
    modifiedAt: string; // Same as above
    tenantId: number | null;
  }
  
export const endpoints = {
  key: '/api/certificate',
};

export async function getCertificates(): Promise<Certificate[]> {
  const response = await fetch(endpoints.key, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok getting the countries');
  }

  const data: Certificate[] = await response.json();
  return data;
}