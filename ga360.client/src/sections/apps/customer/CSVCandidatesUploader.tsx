import React, { useState } from 'react';
import { Box, Typography, Button, useTheme, Stack, useMediaQuery, TableRow, TableCell, TableBody, TableContainer, Table, TableHead, Paper } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import Papa from 'papaparse';
import MultipleFileUploader from 'components/MultipleFileUploader';
import { batchUploadCustomers, CustomerUpload,CustomerUploadResponse } from 'api/customer';
import MainCard from 'components/MainCard';



const CSVCandidatesUploader = () => {
  const [customers, setCustomers] = useState<CustomerUpload[]>([]);
  const theme = useTheme();
  const downSM = useMediaQuery(theme.breakpoints.down('sm'));
  const [uploadedEmails,setUploadedEmails]  = useState<string[]>([]);
  const [uploadSuccess, setUploadSuccess] = useState<boolean>(false);

  const handleFilesUpload = (files: File[]) => {
    if (files.length > 0) {
      const file = files[0];
      Papa.parse(file, {
        header: true,
        skipEmptyLines: true,
        complete: (result) => {
          const data: CustomerUpload[] = result.data.map((row: any, index: number) => ({
            id: index + 1,
            FirstName: row['FirstName'] || '',
            LastName: row['LastName'] || '',
            Name: row['Name'] || '',
            Gender: row['Gender'] || '',
            Age: parseInt(row['Age'], 10) || 0,
            Contact: row['Contact'] || '',
            Email: row['Email'] || '',
            Country: row['Country'] || '',
            Location: row['Location'] || '',
            FatherName: row['FatherName'] || '',
            Role: row['Role'] || '',
            About: row['About'] || '',
            Status: parseInt(row['Status'], 10) || 0,
            Time: row['Time'] || '',
            Date: row['Date'] || '',
            CountryName: row['CountryName'] || '',
            Portfolio: row['Portfolio'] || '',
            DOB: row['DOB'] || '',
            Street: row['Street'] || '',
            City: row['City'] || '',
            Number: row['Number'] || '',
            Postcode: row['Postcode'] || '',
            DateOfBirth: row['DateOfBirth'] || '',
            Ethnicity: row['Ethnicity'] || '',
            Disability: row['Disability'] || '',
            EmployeeStatus: row['EmployeeStatus'] || '',
            Employer: row['Employer'] || '',
            TrainingCentre: row['TrainingCentre'] || '',
            NationalInsurance: row['NationalInsurance'] || ''
          }));
          setCustomers(data);
        }
      });
    }
  };

  const handleUploadCustomers = async () => {
    try {
      const response = await batchUploadCustomers(customers);
      console.log("MYRESP",response)
      if (response.customers && response.customers.length>0) {
        const emails = response.customers.map((customer: CustomerUploadResponse) => customer.email);
        console.log(emails)
        setUploadedEmails(emails);
        setUploadSuccess(true);
      }
    } catch (error) {
      console.error('Error uploading customers:', error);
    }
  };
  
  const columns: GridColDef[] = [
    { field: 'id', headerName: 'ID', width: 90 },
    { field: 'FirstName', headerName: 'First name', width: 150, editable: true },
    { field: 'LastName', headerName: 'Last name', width: 150, editable: true },
    { field: 'Name', headerName: 'Name', width: 150, editable: true },
    { field: 'Gender', headerName: 'Gender', width: 100, editable: true },
    { field: 'Age', headerName: 'Age', type: 'number', width: 110, editable: true },
    { field: 'Contact', headerName: 'Contact', width: 150, editable: true },
    { field: 'Email', headerName: 'Email', width: 200, editable: true },
    { field: 'Country', headerName: 'Country', width: 150, editable: true },
    { field: 'Location', headerName: 'Location', width: 150, editable: true },
    { field: 'FatherName', headerName: 'Father Name', width: 150, editable: true },
    { field: 'Role', headerName: 'Role', width: 150, editable: true },
    { field: 'About', headerName: 'About', width: 200, editable: true },
    { field: 'Status', headerName: 'Status', width: 100, type: 'number', editable: true },
    { field: 'Time', headerName: 'Time', width: 100, editable: true },
    { field: 'Date', headerName: 'Date', width: 150, editable: true },
    { field: 'CountryName', headerName: 'Country Name', width: 150, editable: true },
    { field: 'Portfolio', headerName: 'Portfolio', width: 200, editable: true },
    { field: 'DOB', headerName: 'DOB', width: 150, editable: true },
    { field: 'Street', headerName: 'Street', width: 150, editable: true },
    { field: 'City', headerName: 'City', width: 150, editable: true },
    { field: 'Number', headerName: 'Number', type: 'number', width: 100, editable: true },
    { field: 'Postcode', headerName: 'Postcode', width: 100, editable: true },
    { field: 'DateOfBirth', headerName: 'Date of Birth', width: 150, editable: true },
    { field: 'Ethnicity', headerName: 'Ethnicity', width: 150, editable: true },
    { field: 'Disability', headerName: 'Disability', width: 150, editable: true },
    { field: 'EmployeeStatus', headerName: 'Employee Status', width: 150, editable: true },
    { field: 'Employer', headerName: 'Employer', width: 150, editable: true },
    { field: 'TrainingCentre', headerName: 'Training Centre', width: 200, editable: true },
    { field: 'NationalInsurance', headerName: 'National Insurance', width: 200, editable: true }
  ];

  return (
    <MainCard content={false}>
    <Box p={3} textAlign="center">
    {(customers.length == 0 && !uploadSuccess) && (
  <div>
    <Typography variant="body1" color="textSecondary" gutterBottom>
      Please upload a CSV file containing candidate details. Ensure the file is in CSV format and includes all relevant candidate information.
    </Typography>
    <Typography variant="body2" color="textSecondary" style={{ opacity: 0.7 }} gutterBottom>
      Only CSV files are allowed.
    </Typography>
    <MultipleFileUploader onFilesUpload={handleFilesUpload} acceptedFiles=".csv" />

    <TableContainer component={Paper} style={{ marginTop: '1em', marginBottom: '1em' }}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Column Name</TableCell>
            <TableCell>Description and Example</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {[
            { name: 'FirstName', description: 'First name of the candidate (e.g., John)' },
            { name: 'LastName', description: 'Last name of the candidate (e.g., Doe)' },
            { name: 'Name', description: 'Full name of the candidate (e.g., John Doe)' },
            { name: 'Gender', description: 'Gender of the candidate (e.g., Male, Female)' },
            { name: 'Age', description: 'Age of the candidate (e.g., 28)' },
            { name: 'Contact', description: 'Contact number of the candidate (e.g., +1 (555) 555-5555)' },
            { name: 'Email', description: 'Email address of the candidate (e.g., john.doe@example.com)' },
            { name: 'Country', description: 'Country of residence (e.g., United States)' },
            { name: 'Location', description: 'Location or city of residence (e.g., New York)' },
            { name: 'FatherName', description: 'Father\'s name of the candidate (e.g., Michael Doe)' },
            { name: 'Role', description: 'Role or job position (e.g., Software Engineer)' },
            { name: 'About', description: 'Brief description or bio of the candidate (e.g., Experienced software developer)' },
            { name: 'Status', description: 'Application status (e.g., pending, verified, rejected)' },
            { name: 'Time', description: 'Preferred contact time (e.g., 9:00 AM)' },
            { name: 'Date', description: 'Date of application or submission (e.g., 2023-01-01)' },
            { name: 'CountryName', description: 'Name of the country (e.g., USA)' },
            { name: 'Portfolio', description: 'Link to the candidate\'s portfolio (e.g., http://portfolio.example.com)' },
            { name: 'DOB', description: 'Date of birth (e.g., 1990-01-01)' },
            { name: 'Street', description: 'Street address (e.g., 123 Main St)' },
            { name: 'City', description: 'City of residence (e.g., San Francisco)' },
            { name: 'Number', description: 'House or apartment number (e.g., 10)' },
            { name: 'Postcode', description: 'Postal code (e.g., 94105)' },
            { name: 'DateOfBirth', description: 'Complete date of birth (e.g., 1990-01-01)' },
            { name: 'Ethnicity', description: 'Ethnicity of the candidate (e.g., Asian)' },
            { name: 'Disability', description: 'Disability status (e.g., None)' },
            { name: 'EmployeeStatus', description: 'Current employment status (e.g., Employed)' },
            { name: 'Employer', description: 'Current or previous employer (e.g., ABC Corp)' },
            { name: 'TrainingCentre', description: 'Training centre attended (e.g., XYZ Training Centre)' },
            { name: 'NationalInsurance', description: 'National insurance number (e.g., AB123456C)' },
          ].map((column) => (
            <TableRow key={column.name}>
              <TableCell>{column.name}</TableCell>
              <TableCell>{column.description}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  </div>
)}

      
      {(customers.length > 0 && !uploadSuccess) && (
        <Box mt={3} sx={{ height: 520}}>
          <div style={{ height: 400, width: '100%' }}>
            <DataGrid
              rows={customers}
              columns={columns}
              initialState={{
                pagination: {
                  paginationModel: {
                    pageSize: 5,
                  },
                },
                selection: {
                  selectedRows: customers.map((_, index) => index), // Select all rows
                },
              }}
              pageSizeOptions={[5]}
              checkboxSelection
              disableRowSelectionOnClick
              autoHeight={false}
              sx={{
                
                '& .MuiDataGrid-viewport': {
                  overflow: 'auto',
                },
                '& .MuiDataGrid-root': {
                  overflowX: 'auto',
                },
                '& .MuiDataGrid-row': {
                  overflowX: 'auto',
                },
                '& .MuiDataGrid-columnHeaders': {
                    borderBottom: `1px solid ${theme.palette.divider}`,
                    borderTop: `1px solid ${theme.palette.divider}`,
                    padding: 0, // Remove padding from the header
                    '& .MuiDataGrid-columnHeaderTitle': {
                      fontWeight: 'bold',
                    },
                    '& .MuiDataGrid-columnHeaderTitle:first-child': {
                      paddingLeft: 1, // Set the padding of the first column header
                    },  
                  },
                  '& .MuiDataGrid-cell': {
                    borderColor: theme.palette.divider,
                    padding: theme.spacing(1), // Add padding to the rows
                    paddingLeft: 2,
                  },
                  "& .MuiDataGrid-pagination": {
                    borderTop: "1px solid",
                    marginTop: theme.spacing(2), // Add some margin for better separation
                  },
                    
              }}
            />
          </div>
          <Button variant="contained" color="primary" onClick={() => handleUploadCustomers()}>Upload Candidates</Button>
        </Box>
      )}

{uploadSuccess && (
        <Box mt={3}>
          <Typography variant="body1" color="textSecondary" gutterBottom>
            The following uploads have been processed successfully:
          </Typography>
          <TableContainer component={Paper} style={{ marginTop: '1em', marginBottom: '1em' }}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Email</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {uploadedEmails.map((email, index) => (
                  <TableRow key={index}>
                    <TableCell>{email}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Box>
      )}
    </Box>
    </MainCard>
  );
};

export default CSVCandidatesUploader;
