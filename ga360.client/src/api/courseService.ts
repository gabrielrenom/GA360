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
    sector: string;
    trainingCentreId: number;
    trainingCentre: string;
    price?: number
  }

  export interface CourseDetails extends Course {
    learners: number;
  }
  
  export interface CourseTrainingCentre {
    id: number;
    name: string;
    description: string;
    registrationDate: Date;
    expectedDate: Date;
    duration: number;
    certificateDate: Date;
    certificateNumber: string;
    status: number;
    sector: string;
    assessor: string;
    learners: number;
  }

export const endpoints = {
  key: '/api/course',
  coursetrainingcentre: '/api/course/GetCoursesByTrainingId',
  courseuserid: '/api/course/userid',
  coursedetails: '/api/course/details',
};

export async function getCoursesByTrainingCentreId(trainingCentreId: number): Promise<CourseTrainingCentre[]> {
  const response = await fetch(endpoints.coursetrainingcentre+"/"+trainingCentreId.toString(), {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data: CourseTrainingCentre[] = await response.json();
  return data;
}

export async function getCoursesDetails(): Promise<CourseDetails[]> {
  const response = await fetch(endpoints.coursedetails, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-CSRF': 'Dog',
    },
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data: CourseDetails[] = await response.json();
  return data;
}

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

// export async function getCsrfToken(): Promise<string> {
//   const response = await fetch('/api/csrf/get-csrf-token', {
//     method: 'GET',
//     headers: {
//       'Content-Type': 'application/json',
//     },
//   });

//   if (!response.ok) {
//     throw new Error('Failed to fetch CSRF token');
//   }

//   const data = await response.json();
//   return data.csrfToken;
// }

// export async function getCourses(): Promise<Course[]> {
//   const csrfToken = await getCsrfToken();

//   const response = await fetch(endpoints.key, {
//     method: 'GET',
//     headers: {
//       'Content-Type': 'application/json',
//       'X-CSRF-TOKEN': csrfToken,
//     },
//   });

//   if (!response.ok) {
//     throw new Error('Network response was not ok');
//   }

//   const data: Course[] = await response.json();
//   return data;
// }


export async function getCourseByUserId(id: number): Promise<Course[]> {
  const response = await fetch(`${endpoints.courseuserid}/${id}`, {
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


export async function updateCourse(id: number, course: Course): Promise<Course> {
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

  const updatedCourse: Course = await response.json();
  return updatedCourse;
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
