export interface Course {
    id: number;
    name: string;
    description: string;
    registrationDate: Date;
    expectedDate: Date;
    duration: number;
    certificateDate: Date;
    certificateNumber: string;
    status: number;
  }

export const endpoints = {
  key: '/api/course',
};

export async function getCourses(): Promise<Course[]> {
  const response = await fetch(endpoints.key, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data: Course[] = await response.json();
  return data;
}

export async function getCourse(id: number): Promise<Course> {
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

  const data: Course = await response.json();
  return data;
}

export async function addCourse(course: Course): Promise<Course> {
  console.log("Adding course", course)
  const response = await fetch(endpoints.key, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
    body: JSON.stringify(course),
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const createdCourse: Course = await response.json(); 
  return createdCourse;
}


export async function updateCourse(id: number, course: Course): Promise<void> {
  const response = await fetch(`${endpoints.key}/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
    body: JSON.stringify(course),
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
}

export async function deleteCourse(id: number): Promise<void> {
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
