import React, { useState, useEffect } from 'react';
import { MenuItem, TextField, Box } from '@mui/material';
import { Course, getCourses } from 'api/courseService';

interface CourseDropdownProps {
  value: string;
  onChange: (value: string) => void;
}

const CourseDropdown: React.FC<CourseDropdownProps> = ({ value, onChange }) => {
  const [courses, setCourses] = useState<Course[]>([]);
  const [selectedValue, setSelectedValue] = useState<string>(''); // Use local state for selected value

  useEffect(() => {
    const fetchCourses = async () => {
      if (courses.length === 0) {
        try {
          const courseData = await getCourses();
          setCourses(courseData);

          // Set the initial selected value if it matches one of the options
          const matchedCourse = courseData.find(course => course.name === value);
          if (matchedCourse) {
            setSelectedValue(matchedCourse.id.toString());
            onChange(matchedCourse.id.toString()); // Ensure parent component is updated
          }
        } catch (error) {
          console.error("Failed to fetch courses", error);
        }
      } else {
        // Set the initial selected value if it matches one of the already fetched options
        const matchedCourse = courses.find(course => course.name === value);
        if (matchedCourse) {
          setSelectedValue(matchedCourse.id.toString());
          onChange(matchedCourse.id.toString()); // Ensure parent component is updated
        }
      }
    };

    fetchCourses();
  }, [value, onChange, courses]);

  const handleChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    const newValue = event.target.value as string;
    setSelectedValue(newValue);
    onChange(newValue);
  };

  return (
    <Box display="flex" flexDirection="column" width="100%" style={{ paddingLeft: '10px' }}>
      <TextField
        select
        label="Course"
        value={selectedValue}
        onChange={handleChange}
        variant="outlined"
        fullWidth
        style={{ width: '150px', paddingTop: '10px', backgroundColor: 'white' }}
        InputLabelProps={{ style: { marginTop: '10px' } }}
      >
        {courses.map((course) => (
          <MenuItem key={course.id} value={course.id.toString()}>
            {course.name}
          </MenuItem>
        ))}
      </TextField>
    </Box>
  );
};

export default CourseDropdown;
