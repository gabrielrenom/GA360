import React from 'react';
import { Container, Typography, Box, Paper } from '@mui/material';

const GettingStarted = () => {
  return (
    <Container>
      <Box mt={4}>
        <Paper elevation={3} style={{ padding: '20px', marginBottom: '20px' }}>
          <Typography variant="h3" gutterBottom>Welcome to Global Alliance 360 API</Typography>
          <Typography variant="body1" gutterBottom>
            Global Alliance 360 API provides a comprehensive set of endpoints to interact with our platform. It is designed to enable seamless integration with various applications, offering functionality to manage certificates, countries, courses, customers, ethnicities, qualifications, skills, and training centres.
          </Typography>
          <Typography variant="body1" gutterBottom>
            Whether you are building a new application, integrating with an existing system, or automating workflows, our API aims to provide the tools and endpoints you need to get the job done efficiently.
          </Typography>
          <Typography variant="body1" gutterBottom>
            This guide will walk you through the available endpoints, provide examples of how to use them, and help you get started with integrating our API into your projects.
          </Typography>
        </Paper>
      </Box>

      <Box mt={4}>
        <Paper elevation={2} style={{ padding: '20px' }}>
          <Typography variant="h4" gutterBottom>API Endpoints</Typography>
          
          <Typography variant="h5" mt={2}>Certificates</Typography>
          <Typography variant="body1">GET /api/Certificate - Retrieves a list of certificates.</Typography>

          <Typography variant="h5" mt={2}>Countries</Typography>
          <Typography variant="body1">GET /api/Country/list - Retrieves a list of countries.</Typography>

          <Typography variant="h5" mt={2}>Courses</Typography>
          <Typography variant="body1">GET /api/Course - Retrieves a list of courses.</Typography>
          <Typography variant="body1">POST /api/Course - Creates a new course.</Typography>
          <Typography variant="body1">GET /api/Course/{'{id}'} - Retrieves details of a specific course by ID.</Typography>
          <Typography variant="body1">PUT /api/Course/{'{id}'} - Updates a specific course by ID.</Typography>
          <Typography variant="body1">DELETE /api/Course/{'{id}'} - Deletes a specific course by ID.</Typography>

          <Typography variant="h5" mt={2}>Customers</Typography>
          <Typography variant="body1">GET /api/Customer/list - Retrieves a list of customers.</Typography>
          <Typography variant="body1">GET /api/Customer/list/basic - Retrieves a basic list of customers.</Typography>
          <Typography variant="body1">GET /api/Customer/customerswithcoursequalificationrecords - Retrieves customers with course qualification records.</Typography>
          <Typography variant="body1">POST /api/Customer/customerswithcoursequalificationrecords - Creates a customer with course qualification records.</Typography>
          <Typography variant="body1">PUT /api/Customer/customerswithcoursequalificationrecords/{'{id}'} - Updates a customer with course qualification records by ID.</Typography>
          <Typography variant="body1">DELETE /api/Customer/customerswithcoursequalificationrecords/{'{id}'} - Deletes a customer with course qualification records by ID.</Typography>
          <Typography variant="body1">GET /api/Customer/get - Retrieves a customer.</Typography>
          <Typography variant="body1">GET /api/Customer/get/basic - Retrieves basic customer information.</Typography>
          <Typography variant="body1">POST /api/Customer/create - Creates a new customer.</Typography>
          <Typography variant="body1">PUT /api/Customer/update/{'{id}'} - Updates a customer by ID.</Typography>
          <Typography variant="body1">PUT /api/Customer/updatewithdocuments/{'{id}'} - Updates a customer with documents by ID.</Typography>
          <Typography variant="body1">DELETE /api/Customer/delete/{'{id}'} - Deletes a customer by ID.</Typography>

          <Typography variant="h5" mt={2}>Ethnicities</Typography>
          <Typography variant="body1">GET /api/Ethnicity/list - Retrieves a list of ethnicities.</Typography>

          <Typography variant="h5" mt={2}>Menu</Typography>
          <Typography variant="body1">GET /Menu - Retrieves the menu.</Typography>
          <Typography variant="body1">GET /Menu/landing - Retrieves the landing menu.</Typography>
          <Typography variant="body1">GET /Menu/authnlanding - Retrieves the authentication landing menu.</Typography>

          <Typography variant="h5" mt={2}>Qualifications</Typography>
          <Typography variant="body1">GET /api/Qualification - Retrieves a list of qualifications.</Typography>
          <Typography variant="body1">POST /api/Qualification - Creates a new qualification.</Typography>
          <Typography variant="body1">GET /api/Qualification/qualificationstatuses - Retrieves a list of qualification statuses.</Typography>
          <Typography variant="body1">GET /api/Qualification/{'{id}'} - Retrieves details of a specific qualification by ID.</Typography>
          <Typography variant="body1">PUT /api/Qualification/{'{id}'} - Updates a specific qualification by ID.</Typography>
          <Typography variant="body1">DELETE /api/Qualification/{'{id}'} - Deletes a specific qualification by ID.</Typography>

          <Typography variant="h5" mt={2}>Skills</Typography>
          <Typography variant="body1">GET /api/Skill/list - Retrieves a list of skills.</Typography>

          <Typography variant="h5" mt={2}>Training Centres</Typography>
          <Typography variant="body1">GET /api/TrainingCentre/list - Retrieves a list of training centres.</Typography>
          <Typography variant="body1">GET /api/TrainingCentre - Retrieves training centre details.</Typography>
          <Typography variant="body1">POST /api/TrainingCentre - Creates a new training centre.</Typography>
          <Typography variant="body1">GET /api/TrainingCentre/{'{id}'} - Retrieves details of a specific training centre by ID.</Typography>
          <Typography variant="body1">PUT /api/TrainingCentre/{'{id}'} - Updates a specific training centre by ID.</Typography>
          <Typography variant="body1">DELETE /api/TrainingCentre/{'{id}'} - Deletes a specific training centre by ID.</Typography>
        </Paper>
      </Box>

      <Box mt={4}>
        <Paper elevation={2} style={{ padding: '20px' }}>
          <Typography variant="h4" gutterBottom>Using the API</Typography>
          <Typography variant="body1" gutterBottom>
            To use the API, you'll need to make HTTP requests to the endpoints listed above. You can use tools like Postman or curl to test the endpoints.
          </Typography>
          <Typography variant="body1" gutterBottom>
            In a React application, you can use libraries like Axios to make these requests. Below is an example of how to fetch data from the Certificate endpoint:
          </Typography>
          <pre style={{ background: '#f5f5f5', padding: '10px' }}>
            {`
import axios from 'axios';

const getCertificates = async () => {
  try {
    const response = await axios.get('http://localhost:5000/api/Certificate');
    console.log(response.data);
  } catch (error) {
    console.error('Error fetching certificates:', error);
  }
};

getCertificates();
            `}
          </pre>
        </Paper>
      </Box>
    </Container>
  );
};

export default GettingStarted;
