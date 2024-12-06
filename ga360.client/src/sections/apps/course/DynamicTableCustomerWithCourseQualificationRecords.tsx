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
  getAllCustomerWithCourseQualificationRecords,
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
import CourseDropdown from "./components/CourseDropdown";
import QualificationDropdown from "./components/QualificationDropdown";
import EmailDropdown from "./components/EmailDropdown";
// import EmailDropdown from "./components/EmailDropdown";


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


export default function DynamicTableCustomerWithCourseQualificationRecords() {
  const theme = useTheme();
  const downSM = useMediaQuery(theme.breakpoints.down("sm"));

  const [rows, setRows] = React.useState<CustomersWithCourseQualificationRecordsViewModel[]>([]);
  const [rowModesModel, setRowModesModel] = React.useState<GridRowModesModel>({});

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await getAllCustomerWithCourseQualificationRecords();
        console.log(data)
        setRows(data);
      } catch (error) {
        console.error("Failed to fetch data", error);
      }
    };

    fetchData();
  }, []);

  const handleRowEditStop: GridEventListener<"rowEditStop"> = (params, event) => {
    if (params.reason === GridRowEditStopReasons.rowFocusOut) {
      event.defaultMuiPrevented = true;
    }
  };

  const handleEditClick = (id: GridRowId) => () => {
    const rowToEdit = rows.find((row) => row.id === id);
    console.log("ROWTOEDIT", rowToEdit)
    setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.Edit } });
  };

  const handleSaveClick = (id: GridRowId) => async () => {
    setRowModesModel((prevModel) => ({
      ...prevModel,
      [id]: { mode: GridRowModes.View },
    }));
  
    const rowToUpdate = rows.find((row) => row.id === id);
    console.log("ROWTOUPDATE", rowToUpdate)
    if (rowToUpdate) {
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
    console.log("UPDATED", updatedRow)
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
        console.log("Adding...")
      const createdRow = await createCustomersWithCourseQualificationRecords(newCustomerQualification);
      setRows((prevRows) => prevRows.map((row) => (row.id === newRow.id ? createdRow : row)));
      }
    } 
    else {
      console.log("updating...",newCustomerQualification)
      const updatedRowData = await updateCustomersWithCourseQualificationRecords(Number(newRow.id), newCustomerQualification);
      setRows((prevRows) => prevRows.map((row) => (row.id === newRow.id ? updatedRowData : row)));
    }
    return updatedRow;
  } catch (error) {
    console.error("Failed to save record", error);
    return newRow;
  }
};
  
  const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
    setRowModesModel(newRowModesModel);
  };

  
  const columns: GridColDef[] = [
    { field: "id", headerName: "#", flex: 1, editable: true, type: "number" },
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
            disableRowSelectionOnClick 
            onCellDoubleClick={(params, event) => { event.stopPropagation(); }}
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

