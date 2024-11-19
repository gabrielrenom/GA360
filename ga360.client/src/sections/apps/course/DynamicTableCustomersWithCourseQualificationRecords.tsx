import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import { useTheme } from '@mui/material/styles';
import {
  PlusOutlined,
  EditOutlined,
  DeleteOutlined,
  SaveOutlined,
  CloseOutlined,
} from "@ant-design/icons";
import {
  DataGrid,
  GridRowsProp,
  GridRowModesModel,
  GridRowModes,
  GridColDef,
  GridToolbarContainer,
  GridActionsCellItem,
  GridEventListener,
  GridRowId,
  GridRowModel,
  GridRowEditStopReasons,
  GridSlots,
} from "@mui/x-data-grid";
import MainCard from "components/MainCard";
import { Autocomplete, Stack, TextField, Tooltip, useMediaQuery } from "@mui/material";
import IconButton from "components/@extended/IconButton";

import {
  getAllCustomersWithCourseQualificationRecords,
  CustomersWithCourseQualificationRecordsViewModel,
  deleteCustomersWithCourseQualificationRecords,
  createCustomersWithCourseQualificationRecords,
  updateCustomersWithCourseQualificationRecords,
} from "api/qualificationService";

import { addQualification, deleteQualification, getQualifications, Qualification, updateQualification } from "api/qualificationService";
import { BasicCustomer } from "types/customer";
import { getBasicCandidates } from "api/customer";
import { useEffect, useState } from "react";
import { Course, getCourses } from "api/courseService";
import { Certificate, getCertificates } from "api/certificateService";
import TrainingCentreDropdown from "./components/TrainingCentreDropdown";
import QualificationStatusDropdown from "./components/QualificationStatusDropdown";
import CertificateDropdown from "./components/CertificateDropdown";


interface EditToolbarProps {
  setRows: (newRows: (oldRows: GridRowsProp) => GridRowsProp) => void;
  setRowModesModel: (
    newModel: (oldModel: GridRowModesModel) => GridRowModesModel
  ) => void;
}

function EditToolbar(props: EditToolbarProps) {
  const { setRows, setRowModesModel } = props;

  const handleClick = () => {
    const id = Math.random().toString(36).substr(2, 9);
    const newRow = {
      id,
      customerId: 0,
      email: "",
      courseId: undefined,
      courseName: "",
      qualificationId: undefined,
      qualificationName: "",
      certificateId: undefined,
      certificateName: "",
      trainingCentreId: undefined,
      trainingCentre: "",
      progression: 0,
      qualificationStatusId: undefined,
      qualificationStatus: "",
      isNew: true,
    };

    // Prepend the new row to the rows array
    setRows((oldRows) => [newRow, ...oldRows]);
    setRowModesModel((oldModel) => ({
      ...oldModel,
      [id]: { mode: GridRowModes.Edit, fieldToFocus: "email" },
    }));
  };

  return (
    <GridToolbarContainer sx={{ justifyContent: "flex-end", paddingBottom: 2, paddingRight: 2 }}>
      <Button
        variant="contained"
        color="primary"
        startIcon={<PlusOutlined />}
        onClick={handleClick}
      >
        Add Record
      </Button>
    </GridToolbarContainer>
  );
}


export default function DynamicTableCustomersWithCourseQualificationRecords() {
  const theme = useTheme();
  const downSM = useMediaQuery(theme.breakpoints.down("sm"));

  const [rows, setRows] = React.useState<CustomersWithCourseQualificationRecordsViewModel[]>([]);
  const [rowModesModel, setRowModesModel] = React.useState<GridRowModesModel>({});
  const [courses, setCourses] = useState<Course[]>([]);
  const [qualifications, setQualifications] = useState<Qualification[]>([]);
  // const [certificates, setCertificates] = useState<Certificate[]>([]);

  // useEffect(() => {
  //   const fetchCertificates = async () => {
  //     try {
  //       const data = await getCertificates();
  //       setCertificates(data);
  //     } catch (error) {
  //       console.error("Failed to fetch certificates", error);
  //     }
  //   };

  //   fetchCertificates();
  // }, []);

  useEffect(() => {
    const fetchQualifications = async () => {
      try {
        const qualificationData = await getQualifications();
        setQualifications(qualificationData);
      } catch (error) {
        console.error("Failed to fetch qualifications", error);
      }
    };

    fetchQualifications();
  }, []);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await getAllCustomersWithCourseQualificationRecords();
        console.log(data)
        setRows(data);
      } catch (error) {
        console.error("Failed to fetch data", error);
      }
    };

    fetchData();
  }, []);

  const [emails, setEmails] = useState<BasicCustomer[]>([]);

useEffect(() => {
  const fetchEmails = async () => {
    try {
      const emailData = await getBasicCandidates();
      setEmails(emailData);
    } catch (error) {
      console.error("Failed to fetch emails", error);
    }
  };

  fetchEmails();
}, []);

useEffect(() => {
  const fetchCourses = async () => {
    try {
      const courseData = await getCourses();
      setCourses(courseData);
    } catch (error) {
      console.error("Failed to fetch courses", error);
    }
  };

  fetchCourses();
}, []);

  const handleRowEditStop: GridEventListener<"rowEditStop"> = (params, event) => {
    if (params.reason === GridRowEditStopReasons.rowFocusOut) {
      event.defaultMuiPrevented = true;
    }
  };

  const handleEditClick = (id: GridRowId) => () => {
    setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.Edit } });
  };

  const handleSaveClick = (id: GridRowId) => async () => {
    // Set row mode to View
    setRowModesModel((prevModel) => ({
      ...prevModel,
      [id]: { mode: GridRowModes.View },
    }));
  
    // Get the row to update
    const rowToUpdate = rows.find((row) => row.id === id);
    if (rowToUpdate) {
      // Perform the save operation if the row is new or updated
      await processRowUpdate(rowToUpdate);
    }
  };
  
  const handleDeleteClick = (id: GridRowId) => async () => {
    try {
      await deleteCustomersWithCourseQualificationRecords(Number(id));
      setRows((prevRows) => prevRows.filter((row) => row.id !== id));
    } catch (error) {
      console.error("Failed to delete record", error);
    }
  };
  

  const handleCancelClick = (id: GridRowId) => () => {
    setRowModesModel({
      ...rowModesModel,
      [id]: { mode: GridRowModes.View, ignoreModifications: true },
    });

    const editedRow = rows.find((row) => row.id === id);
    if (editedRow!.isNew) {
      setRows(rows.filter((row) => row.id !== id));
    }
  };

  
const processRowUpdate = async (newRow: GridRowModel) => {
  const updatedRow = { ...newRow, isNew: false };
  try {
    const newCustomerQualification = updatedRow as unknown as CustomersWithCourseQualificationRecordsViewModel;
    
    // Initialize the ID to 0 for new rows
    if (newRow.isNew) {
      newCustomerQualification.id = 0;
    }

    // Convert certificateName to certificateId if it is not null, undefined, or an empty string
    if (newCustomerQualification.certificateName) {
      newCustomerQualification.certificateId = Number(newCustomerQualification.certificateName);
    }

    // Convert courseName to courseId
    if (newCustomerQualification.courseName) {
      newCustomerQualification.courseId = Number(newCustomerQualification.courseName);
    }

    // Convert qualificationName to qualificationId
    if (newCustomerQualification.qualificationName) {
      newCustomerQualification.qualificationId = Number(newCustomerQualification.qualificationName);
    }

    // Convert email to customerId
    if (newCustomerQualification.email) {
      newCustomerQualification.customerId = Number(newCustomerQualification.email);
    }

    // Convert qualificationStatus to qualificationStatusId
    if (newCustomerQualification.qualificationStatus) {
      newCustomerQualification.qualificationStatusId = Number(newCustomerQualification.qualificationStatus);
    }

    // Perform the save operation based on the state of the row
    if (newRow.isNew) {
      if(newCustomerQualification.isNew == false)
      {
      const createdRow = await createCustomersWithCourseQualificationRecords(newCustomerQualification);
      setRows((prevRows) => prevRows.map((row) => (row.id === newRow.id ? createdRow : row)));
      }
    } 
    // else {
    //   const updatedRowData = await updateCustomersWithCourseQualificationRecords(Number(newRow.id), newCustomerQualification);
    //   setRows((prevRows) => prevRows.map((row) => (row.id === newRow.id ? updatedRowData : row)));
    // }
    return updatedRow;
  } catch (error) {
    console.error("Failed to save record", error);
    return newRow;
  }
};
  
  const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
    setRowModesModel(newRowModesModel);
  };

  
  const emailOptions = emails.map(email => ({ label: email.email, value: email.id.toString() }));

  interface EmailDropdownProps {
    value: string;
    onChange: (value: string) => void;
    options: { label: string, value: string }[];
  }

  const EmailDropdown: React.FC<EmailDropdownProps> = ({ value, onChange, options }) => {
    const handleChange = (event: any, newValue: any) => {
      onChange(newValue ? newValue.value : '');
    };
  
    return (
      <Box display="flex" flexDirection="column" width="100%"  style={{ paddingLeft: '10px'}}>
        <Autocomplete
          value={options.find(option => option.value === value) || null}
          onChange={handleChange}
          options={options}
          getOptionLabel={(option) => option.label}
          isOptionEqualToValue={(option, value) => option.value === value}
          renderInput={(params) => (
            <TextField
              {...params}
              label="Email"
              variant="outlined"
              style={{ width: '150px', paddingTop: '10px', backgroundColor: 'white' }} // Set background color here
              InputLabelProps={{ style: { marginTop: '10px' } }} // Adjust label margin here
            />
          )}
          PaperComponent={(props) => (
            <div {...props} style={{ ...props.style, maxWidth: '100%', minWidth: '300px', backgroundColor: 'white' }} /> // Set background color here
          )}
          style={{ width: '300px', flex: 1 }}
        />
      </Box>
    );
  };

  const courseOptions = courses.map(course => ({ label: course.name, value: course.id.toString() }));
  
  interface CourseDropdownProps {
    value: string;
    onChange: (value: string) => void;
    options: { label: string, value: string }[];
  }
  
  const CourseDropdown: React.FC<CourseDropdownProps> = ({ value, onChange, options }) => {
    const handleChange = (event: any, newValue: any) => {
      onChange(newValue ? newValue.value : '');
    };
  
    return (
      <Box display="flex" flexDirection="column" width="100%"  style={{ paddingLeft: '10px'}}>
        <Autocomplete
          value={options.find(option => option.value === value) || null}
          onChange={handleChange}
          options={options}
          getOptionLabel={(option) => option.label}
          isOptionEqualToValue={(option, value) => option.value === value}
          renderInput={(params) => (
            <TextField
              {...params}
              label="Course"
              variant="outlined"
              fullWidth
              style={{ width: '150px', paddingTop: '10px', backgroundColor: 'white' }} // Set background color here
              InputLabelProps={{ style: { marginTop: '10px' } }} // Adjust label margin here
            />
          )}
          PaperComponent={(props) => (
            <div {...props} style={{ ...props.style, maxWidth: '100%', minWidth: '300px', backgroundColor: 'white' }} /> // Set background color here
          )}
          style={{ width: '300px', flex: 1 }}
        />
      </Box>
    );
  };
  
  const qualificationOptions = qualifications.map(qualification => ({ label: qualification.name, value: qualification.id.toString() }));

  interface QualificationDropdownProps {
    value: string;
    onChange: (value: string) => void;
    options: { label: string, value: string }[];
  }
  
  const QualificationDropdown: React.FC<QualificationDropdownProps> = ({ value, onChange, options }) => {
    const handleChange = (event: any, newValue: any) => {
      onChange(newValue ? newValue.value : '');
    };
  
    return (
      <Box display="flex" flexDirection="column" width="100%"  style={{ paddingLeft: '10px'}}>
        <Autocomplete
          value={options.find(option => option.value === value) || null}
          onChange={handleChange}
          options={options}
          getOptionLabel={(option) => option.label}
          isOptionEqualToValue={(option, value) => option.value === value}
          renderInput={(params) => (
            <TextField
              {...params}
              label="Qualification"
              variant="outlined"
              fullWidth
              style={{ width: '150px', paddingTop: '10px', backgroundColor: 'white' }} // Set background color here
              InputLabelProps={{ style: { marginTop: '10px' } }} // Adjust label margin here
            />
          )}
          PaperComponent={(props) => (
            <div {...props} style={{ ...props.style, maxWidth: '100%', minWidth: '300px', backgroundColor: 'white' }} /> // Set background color here
          )}
          style={{ width: '300px', flex: 1 }}
        />
      </Box>
    );
  };

  // interface CertificateDropdownProps {
  //   value: string;
  //   onChange: (value: string) => void;
  // }
  
  // const certificateOptions = certificates.map(cert => ({
  //   label: cert.name,
  //   value: cert.id.toString(),
  // }));
  
  // const CertificateDropdown: React.FC<CertificateDropdownProps> = ({ value, onChange }) => {
  //   const [certificates, setCertificates] = useState<Certificate[]>([]);
  
  //   const handleChange = (event: any, newValue: any) => {
  //     onChange(newValue ? newValue.value : '');
  //   };
  

  
  //   return (
  //     <Box display="flex" flexDirection="column" width="100%"  style={{ paddingLeft: '10px'}}>
  //       <Autocomplete
  //         value={certificateOptions.find(option => option.value === value) || null}
  //         onChange={handleChange}
  //         options={certificateOptions}
  //         getOptionLabel={(option) => option.label}
  //         isOptionEqualToValue={(option, value) => option.value === value}
  //         renderInput={(params) => (
  //           <TextField
  //             {...params}
  //             label="Certificate"
  //             variant="outlined"
  //             fullWidth
  //             style={{ width: '150px', paddingTop: '10px', backgroundColor: 'white' }} // Set background color here
  //             InputLabelProps={{ style: { marginTop: '10px' } }} // Adjust label margin here
  //           />
  //         )}
  //         PaperComponent={(props) => (
  //           <div {...props} style={{ ...props.style, maxWidth: '100%', minWidth: '300px', backgroundColor: 'white' }} /> // Set background color here
  //         )}
  //         style={{ width: '300px', flex: 1 }}
  //       />
  //     </Box>
  //   );
  // };
  
  

  const columns: GridColDef[] = [
    { field: "id", headerName: "#", flex: 1, editable: true, type: "number" },
    { field: "customerId", hideable: true },
    {
      field: 'email',
      headerName: 'EMAIL',
      flex: 2,
      editable: true,
      renderEditCell: (params) => (
        <EmailDropdown
          value={params.value}
          onChange={(value) => params.api.setEditCellValue({ id: params.id, field: params.field, value })}
          options={emailOptions}
        />
      ),
    },
   { field: "courseId", hideable: true },
    {
      field: 'courseName',
      headerName: 'COURSE',
      flex: 2,
      editable: true,
      renderEditCell: (params) => (
        <CourseDropdown
          value={params.value}
          onChange={(value) => params.api.setEditCellValue({ id: params.id, field: params.field, value })}
          options={courseOptions}
        />
      )
    },
    { field: "qualificationId", hideable: false },
    {
      field: 'qualificationName',
      headerName: 'QUALIFICATION',
      flex: 2,
      editable: true,
      renderEditCell: (params) => (
        <QualificationDropdown
          value={params.value}
          onChange={(value) => params.api.setEditCellValue({ id: params.id, field: params.field, value })}
          options={qualificationOptions}
        />
      ),
    },    
    { field: "certificateId", hideable: true },
    {
      field: 'certificateName',
      headerName: 'CERTIFICATE',
      flex: 2,
      editable: true,
      renderEditCell: (params) => (
        <CertificateDropdown
          value={params.value}
          onChange={(value) => params.api.setEditCellValue({ id: params.id, field: params.field, value })}
        />
      ),
    },
    { field: "trainingCentreId", hideable: true },
    {
      field: 'trainingCentre',
      headerName: 'TRAINING CENTRE',
      flex: 2,
      editable: true,
      renderEditCell: (params) => (
        <TrainingCentreDropdown
          value={params.value}
          onChange={(value) => params.api.setEditCellValue({ id: params.id, field: params.field, value })}
        />
      ),
    },    
    { field: "progression", headerName: "PROGRESSION", flex: 1, editable: true },
    { field: "qualificationStatusId", hideable: true },
    {
      field: 'qualificationStatus',
      headerName: 'QUALIFICATION STATUS',
      flex: 2,
      editable: true,
      renderEditCell: (params) => (
        <QualificationStatusDropdown
          value={params.value}
          onChange={(value) => params.api.setEditCellValue({ id: params.id, field: params.field, value })}
        />
      ),
    },
    {
      field: "actions",
      type: "actions",
      headerName: "ACTIONS",
      flex: 1,
      cellClassName: "actions",
      getActions: ({ id }) => {
        const isInEditMode = rowModesModel[id]?.mode === GridRowModes.Edit;

        if (isInEditMode) {
          return [
            <GridActionsCellItem
              icon={<SaveOutlined />}
              label="Save"
              sx={{ color: "primary.main" }}
              onClick={handleSaveClick(id)}
            />,
            <GridActionsCellItem
              icon={<CloseOutlined />}
              label="Cancel"
              className="textPrimary"
              onClick={handleCancelClick(id)}
              color="inherit"
            />,
          ];
        }

        return [
          <GridActionsCellItem
            icon={
              <Tooltip title="Edit">
                <IconButton color="primary">
                  <EditOutlined />
                </IconButton>
              </Tooltip>
            }
            label="Edit"
            className="textPrimary"
            onClick={handleEditClick(id)}
            color="inherit"
          />,
          <GridActionsCellItem
            icon={
              <Tooltip title="Delete">
                <IconButton color="error">
                  <DeleteOutlined />
                </IconButton>
              </Tooltip>
            }
            label="Delete"
            onClick={handleDeleteClick(id)}
            color="inherit"
          />,
        ];
      },
    },
  ];
  return (
    <MainCard content={false}>
      <Stack
        direction={{ xs: "column", sm: "row" }}
        spacing={2}
        alignItems="center"
        justifyContent="space-between"
        sx={{
          paddingTop: 2,
          ...(downSM && { "& .MuiOutlinedInput-root, & .MuiFormControl-root": { width: "100%" } }),
        }}
      >
        <Box
          sx={{
            height: 500,
            width: "100%",
            overflowX: "auto", // Enable horizontal scrolling
            "& .actions": { color: "text.secondary" },
            "& .textPrimary": { color: "text.primary" },
          }}
        >
          <DataGrid
              initialState={{
                columns: {
                  columnVisibilityModel: {
                    customerId: false,
                    courseId: false,
                    qualificationId:false,
                    certificateId: false,
                    trainingCentreId: false,
                    qualificationStatusId: false
                  },
                },
              }}
            rows={rows}
            columns={columns}
            editMode="row"
            rowModesModel={rowModesModel}
            onRowModesModelChange={handleRowModesModelChange}
            onRowEditStop={handleRowEditStop}
            processRowUpdate={processRowUpdate}
            slots={{ toolbar: EditToolbar as GridSlots["toolbar"] }}
            slotProps={{ toolbar: { setRows, setRowModesModel } }}
            sx={{
              "& .MuiDataGrid-columnHeaders": {
                borderBottom: `1px solid ${theme.palette.divider}`,
                borderTop: `1px solid ${theme.palette.divider}`,
                padding: 0, // Remove padding from the header
                "& .MuiDataGrid-columnHeaderTitle": { fontWeight: "bold" },
                "& .MuiDataGrid-columnHeaderTitle:first-child": { paddingLeft: 1 },
              },
              "& .MuiDataGrid-cell": {
                borderColor: theme.palette.divider,
                padding: theme.spacing(1), // Add padding to the rows
                paddingLeft: 2,
              },
              "& .MuiDataGrid-pagination": {
                borderTop: "1px solid",
                marginTop: theme.spacing(2), // Add some margin for better separation
              },
            }}
            rowHeight={60}
          />
        </Box>
      </Stack>
    </MainCard>
  );
}

