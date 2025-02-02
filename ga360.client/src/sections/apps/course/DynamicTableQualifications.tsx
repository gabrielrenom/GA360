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
  GridRowsProp,
  GridRowModesModel,
  GridRowModes,
  DataGrid,
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
import { Stack, Tooltip, useMediaQuery } from "@mui/material";
import IconButton from "components/@extended/IconButton";
import { QualificationModel } from "types/customerApiModel";
import { addQualification, deleteQualification, getQualifications, getQualificationsTrainingCentres, getQualificationsWithTrainingCentresForTable, Qualification, QualificationTable, updateQualification } from "api/qualificationService";
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';
import TrainingCentreForCoursesDropdown from "./components/TrainingCentreForCoursesDropdown";
import { getTrainingCentres, TrainingCentre } from "api/trainingcentreService";
import { useEffect, useState } from "react";
import TrainingCentreForCoursesMultiDropdown from "./components/TrainingCentreForCoursesMultiDropdown";


const initialRows: GridRowsProp = [];

interface EditToolbarProps {
  setRows: (newRows: (oldRows: GridRowsProp) => GridRowsProp) => void;
  setRowModesModel: (
    newModel: (oldModel: GridRowModesModel) => GridRowModesModel
  ) => void;
  trainingCentres: TrainingCentre[]; 
}

function EditToolbar(props: EditToolbarProps) {
  const { setRows, setRowModesModel,trainingCentres } = props;

  const handleClick = () => {
    const id ='';
    const defaultTrainingCentre = trainingCentres.length > 0 ? trainingCentres[0] : null;
    setRows((oldRows) => [
      {
        id,
        name: "",
        description: "",
        duration: 0,
        registration: null,
        expected: null,
        certification: null,
        certificateNumber: 0,
        trainingCentre: defaultTrainingCentre ? defaultTrainingCentre.id.toString() : "", // Set default value
        isNew: true,
      },
      ...oldRows,
    ]);
    setRowModesModel((oldModel) => ({
      ...oldModel,
      [id]: { mode: GridRowModes.Edit, fieldToFocus: "qan" },
    }));
  };
  

  return (
    <GridToolbarContainer sx={{ justifyContent: 'flex-end', paddingBottom: 2, paddingRight: 2 }}>
      <Button
        variant="contained"
        color="primary"
        startIcon={<PlusOutlined />}
        onClick={handleClick}
      >
        Add Qualification
      </Button>
    </GridToolbarContainer>
  );
}

export default function DynamicTableQualifications() {
  const theme = useTheme();
  const downSM = useMediaQuery(theme.breakpoints.down('sm'));

  const [rows, setRows] = useState<GridRowsProp>(initialRows);
  const [rowModesModel, setRowModesModel] = useState<GridRowModesModel>({});

  const [trainingCentres, setTrainingCentres] = useState([]);
  const [trainingCentresForQualification, settrainingCentresForQualification] = useState([]);
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarSeverity, setSnackbarSeverity] = useState("success");
  const [snackbarMessage, setSnackbarMessage] = useState("");

  const showSnackbar = (message, severity) => {
    setSnackbarMessage(message);
    setSnackbarSeverity(severity);
    setSnackbarOpen(true);
  };


  useEffect(() => {
    async function fetchTrainingCentres() {
      const centres = await getTrainingCentres();
      setTrainingCentres(centres);
    }
    fetchTrainingCentres();
  }, []);

  useEffect(() => {
    const fetchCourses = async () => {
      try {
        const qualifications: QualificationTable[] = await getQualificationsWithTrainingCentresForTable(0);
        console.log("QUALUFICATIONS",qualifications)
        setRows(qualifications);
      } catch (error) {
        console.error('Failed to fetch courses:', error);
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

  const handleSaveClick = (id: GridRowId) => () => {
    setRowModesModel((prevRowModesModel) => ({
      ...prevRowModesModel,
      [id]: { mode: GridRowModes.View },
    }));
  };

  const handleDeleteClick = (id: GridRowId) => async () => {
    try {
      await deleteQualification(Number(id));
      setRows(rows.filter((row) => row.id !== id));
    } catch (error) {
      setSnackbarMessage('Failed to delete qualifucation, it is probablu used by a candidate');
      setSnackbarOpen(true);
    }
  };
  
  const handleSnackbarClose = (event?: React.SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') {
      return;
    }
    setSnackbarOpen(false);
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

  const mapToQualification = (row: GridRowModel, trainingCentreIds:number[]): QualificationTable => {
    return {
      id: row.id as number,
      qan: row.qan,
      name: row.name as string,
      registrationDate: row.registrationDate ? new Date(row.registrationDate as string) : null,
      expectedDate: row.expectedDate ? new Date(row.expectedDate as string) : null,
      certificateDate: row.certificateDate ? new Date(row.certificateDate as string) : null,
      certificateNumber: row.certificateNumber as number,
      status: row.status as number,
      trainingCentreId: typeof row.trainingCentreId === 'number' ? row.trainingCentreId : (row.trainingCentre as number),
      trainingCentre: row.trainingCentre,
      internalReference: row.internalReference,
      learners: row.learners,
      awardingBody: row.awardingBody,
      price: row.price,
      sector: row.sector,
      trainingCentreIds: trainingCentreIds
    };
  };
  
  const fetchQualifications = async () => {
    try {
      const qualifications: QualificationTable[] = await getQualificationsWithTrainingCentresForTable(0);
      console.log("QUALIFICATIONS", qualifications);
      setRows(qualifications);
    } catch (error) {
      console.error('Failed to fetch qualifications:', error);
    }
  };
  
  const processRowUpdate = async (newRow: GridRowModel) => {
    const updatedRow = { ...newRow, isNew: false };
    try {
      console.log("SSS", newRow);
      if (newRow.isNew) {

        const qualification = mapToQualification(newRow,trainingCentresForQualification);
        qualification.id = 0;
        const createdQualification = await addQualification(qualification);
        if (createdQualification) {
          showSnackbar("Record created successfully!", "success");
          await fetchQualifications();
        } else {
          showSnackbar("Failed to create record due to dependencies.", "error");
        }
        await fetchQualifications();
      } else {
        const qualification = mapToQualification(newRow,trainingCentresForQualification);
        const updatedQualification = await updateQualification(qualification.id, qualification);
        if (updatedQualification) {
          showSnackbar("Record updated successfully!", "success");
          newRow.trainingCentre = updatedQualification.trainingCentre;
          setRows(rows.map((row) => (row.id === newRow.id ? newRow : row)));
        } else {
          showSnackbar("Failed to update record due to dependencies.", "error");
        }
      }
      settrainingCentresForQualification([]);
    } catch (error) {
      console.error('Failed to update qualification:', error);
    }
    return updatedRow;
  };
  
  

  const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
    setRowModesModel(newRowModesModel);
  };

  const handleTrainingCentreChange = (qualificationId, selectedIds) => {
    settrainingCentresForQualification(selectedIds);
    setRows((prevRows) =>
      prevRows.map((row) =>
        row.id === qualificationId ? { ...row, trainingCentre: selectedIds } : row
      )
    );
  };

const columns: GridColDef[] = [
  {
    field: "id",
    headerName: "#",
    editable: false,
    type: "number",
    headerAlign: 'center',
    align: 'center',
  },
  {
    field: "qan",
    headerName: "QAN",
    editable: true,
    type: "string",
    headerAlign: 'center',
    align: 'center',
  },
  {
    field: "internalReference",
    headerName: "INT REF",
    editable: true,
    type: "string",
    headerAlign: 'center',
    align: 'center',
    flex: 1,

  },
  {
    field: 'name',
    headerName: 'NAME',
    flex: 1,
    editable: true
  },
    {
      field: 'awardingBody',
      headerName: 'AWARDING BODY',
      flex: 1,
      editable: true,
    },
  {
    field: 'trainingCentre',
    headerName: 'TRAINING CENTRE',
    flex: 2,
    editable: true,
    renderEditCell: (params) => (
      <TrainingCentreForCoursesMultiDropdown
        qualificationId = {params.id}
        value={params.value}
        onChange={handleTrainingCentreChange}
        
      />
    ),
  },
  {
    field: "sector",
    headerName: "SECTOR",
    flex: 1,
    editable: true,
    type: "singleSelect",
    valueOptions: [
        { value: "Technology", label: "Technology" },
        { value: "Healthcare", label: "Healthcare" },
        { value: "Finance", label: "Finance" },
        { value: "Education", label: "Education" },
        { value: "Engineering", label: "Engineering" },
        { value: "Marketing", label: "Marketing" },
        { value: "Hospitality", label: "Hospitality" },
        { value: "Retail", label: "Retail" },
        { value: "Manufacturing", label: "Manufacturing" },
        { value: "Construction", label: "Construction" },
    ],
},
  {
    field: 'learners',
    headerName: 'LEARNERS',
    flex: 1,
    editable: false,
  },
  {
    field: 'expectedDate',
    headerName: 'EXP DATE',
    type: 'date',
    flex: 1,
    editable: true,
    valueFormatter: (params) => {
      const date = new Date(params as string);
      return date.toLocaleDateString();
    },
  },
  {
    field: 'certificateNumber',
    headerName: 'CERT NUMBER',
    flex: 1,
    type: "number",
    editable: true,
  },
  
  {
    field: 'status',
    headerName: 'STATUS',
    flex: 1,
    editable: true,
    type: 'singleSelect',
    valueOptions: [
      { value: 1, label: 'Active' },
      { value: 2, label: 'Not Active' },
    ],
  },
  {
    field: 'price',
    headerName: 'PRICE',
    flex: 1,
    editable: true,
  },
  {
    field: 'actions',
    type: 'actions',
    headerName: 'Actions',
    flex: 1,
    cellClassName: 'actions',
    getActions: ({ id }) => {
      const isInEditMode = rowModesModel[id]?.mode === GridRowModes.Edit;

      if (isInEditMode) {
        return [
          <GridActionsCellItem
            icon={<SaveOutlined />}
            label="Save"
            sx={{ color: 'primary.main' }}
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
            direction={{ xs: 'column', sm: 'row' }}
            spacing={2}
            alignItems="center"
            justifyContent="space-between"
            sx={{ paddingTop: 2, ...(downSM && { '& .MuiOutlinedInput-root, & .MuiFormControl-root': { width: '100%' } }) }}
          >
            <Box
              sx={{
                height: 500,
                width: "100%",
                overflowX: "auto", // Enable horizontal scrolling
                "& .actions": {
                  color: "text.secondary",
                },
                "& .textPrimary": {
                  color: "text.primary",
                },
              }}
            >
              <DataGrid
                rows={rows}
                columns={columns}
                editMode="row"
                rowModesModel={rowModesModel}
                onRowModesModelChange={handleRowModesModelChange}
                onRowEditStop={handleRowEditStop}
                processRowUpdate={processRowUpdate}
                disableRowSelectionOnClick 
                onCellDoubleClick={(params, event) => { event.stopPropagation(); }}
                initialState={{
                  columns: {
                    columnVisibilityModel: {
                      certificateDate: false,
                      registrationDate: false,
                      certificateNumber: false
                    },
                  },
                }}
                slots={{
                  toolbar: EditToolbar as GridSlots['toolbar'],
                }}
                slotProps={{
                  toolbar: { setRows, setRowModesModel, trainingCentres },
                }}
                sx={{
                  '& .MuiDataGrid-columnHeaders': {
                    borderBottom: `1px solid ${theme.palette.divider}`,
                    borderTop: `1px solid ${theme.palette.divider}`,
                    fontSize: '8px',

                    padding: 0, // Remove padding from the header
                    '& .MuiDataGrid-columnHeaderTitle': {
                      fontWeight: 'bold',
                    },
                    '& .MuiDataGrid-columnHeaderTitle:first-child': {
                      //paddingLeft: 1, // Set the padding of the first column header
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
            </Box>
          </Stack>
      <Snackbar
        open={snackbarOpen}
        autoHideDuration={6000}
        onClose={handleSnackbarClose}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
      >
        <Alert onClose={handleSnackbarClose} 
        //@ts-ignore
        severity={snackbarSeverity} sx={{ width: '100%' }}>
          {snackbarMessage}
        </Alert>
      </Snackbar>        </MainCard>
      );
    }      