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
import { addQualification, deleteQualification, getQualifications, Qualification, updateQualification } from "api/qualificationService";
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';


const initialRows: GridRowsProp = [];

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
    setRows((oldRows) => [
      {
        id,
        name: "",
        description: "",
        duration: 0,
        registration: null,
        expected: null,
        certification: null,
        isNew: true,
      },
      ...oldRows,
    ]);
    setRowModesModel((oldModel) => ({
      ...oldModel,
      [id]: { mode: GridRowModes.Edit, fieldToFocus: "name" },
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

  const [rows, setRows] = React.useState<GridRowsProp>(initialRows);
  const [rowModesModel, setRowModesModel] = React.useState<GridRowModesModel>({});

  const [snackbarOpen, setSnackbarOpen] = React.useState(false);
  const [snackbarMessage, setSnackbarMessage] = React.useState('');

  React.useEffect(() => {
    const fetchCourses = async () => {
      try {
        const qualifications: Qualification[] = await getQualifications();
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
      console.error('Failed to delete course :', error);
      setSnackbarMessage('Failed to delete course, it is probablu used by a candidate');
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

  const mapToQualification = (row: GridRowModel): Qualification => {
    return {
      id: row.id as number,
      name: row.name as string,
      registrationDate: row.registrationDate ? new Date(row.registrationDate as string) : null,
      expectedDate: row.expectedDate ? new Date(row.expectedDate as string) : null,
      certificateDate: row.certificateDate ? new Date(row.certificateDate as string) : null,
      certificateNumber: row.certificateNumber as number,
      status: row.status as number
    };
  };
  
  const processRowUpdate = async (newRow: GridRowModel) => {
    const updatedRow = { ...newRow, isNew: false };
    try {
      if (newRow.isNew) {
        const qualification = mapToQualification(newRow);
        qualification.id = 0;
        const createdQualification = await addQualification(qualification);
        setRows(rows.map((row) => (row.id === newRow.id ? createdQualification : row)));
      } else {
        const qualification = mapToQualification(newRow);
        await updateQualification(qualification.id, qualification); // Call updateQualification for existing rows
        setRows(rows.map((row) => (row.id === newRow.id ? updatedRow : row)));
      }
    } catch (error) {
      console.error('Failed to update qualification:', error);
    }
    return updatedRow;
  };
  
  

  const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
    setRowModesModel(newRowModesModel);
  };


const columns: GridColDef[] = [
  {
    field: "id",
    headerName: "QAN",
    editable: true,
    type: "number",
    headerAlign: 'center', // Aligns the header text to the center
    align: 'center', // Aligns the cell content to the center
  },
  {
    field: 'name',
    headerName: 'QUALIFICATION NAME',
    flex: 1,
    editable: true,
  },
  {
    field: 'registrationDate',
    headerName: 'REGISTRATION DATE',
    type: 'date',
    flex: 1,
    editable: true,
    valueFormatter: (params) => {
      const date = new Date(params as string);
      return date.toLocaleDateString();
    },
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
    field: 'certificateDate',
    headerName: 'CERTIFICATE DATE?',
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
    headerName: 'CERTIFICATE NUMBER',
    flex: 1,
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
            
                slots={{
                  toolbar: EditToolbar as GridSlots['toolbar'],
                }}
                slotProps={{
                  toolbar: { setRows, setRowModesModel },
                }}
                sx={{
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
            </Box>
          </Stack>
          <Snackbar open={snackbarOpen} autoHideDuration={6000} onClose={handleSnackbarClose} ></Snackbar>
        </MainCard>
      );
    }      