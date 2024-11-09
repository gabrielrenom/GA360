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
import { addTrainingCentre, deleteTrainingCentre, getTrainingCentres, TrainingCentre, TrainingCentreWithAddress } from "api/trainingcentreService";


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
      ...oldRows,
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
        Add Training Centre
      </Button>
    </GridToolbarContainer>
  );
}

export default function DynamicTableTrainingCentre() {
  const theme = useTheme();
  const downSM = useMediaQuery(theme.breakpoints.down('sm'));

  const [rows, setRows] = React.useState<GridRowsProp>(initialRows);
  const [rowModesModel, setRowModesModel] = React.useState<GridRowModesModel>({});

  React.useEffect(() => {
    const fetchTrainingCentres = async () => {
      try {
        const trainingCentres: TrainingCentre[] = await getTrainingCentres();
        console.log("TRAINING CENTRES", trainingCentres);
  
        const trainingCentresWithAddress: TrainingCentreWithAddress[] = trainingCentres.map(mapTrainingCentreToWithAddress);
        setRows(trainingCentresWithAddress);
      } catch (error) {
        console.error('Failed to fetch training centres:', error);
      }
    };
  
    fetchTrainingCentres();
  }, []);
  

  const mapTrainingCentreToWithAddress = (centre: TrainingCentre): TrainingCentreWithAddress => {
    const { id, name, addressId, address } = centre;
    const { street, number, postcode, city } = address;
    return {
      id,
      name,
      addressId,
      street,
      number,
      postcode,
      city
    };
  };
  
  const handleRowEditStop: GridEventListener<"rowEditStop"> = (params, event) => {
    if (params.reason === GridRowEditStopReasons.rowFocusOut) {
      event.defaultMuiPrevented = true;
    }
  };

  const handleEditClick = (id: GridRowId) => () => {
    setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.Edit } });
  };

  
  const handleSaveClick = (id: GridRowId) => async () => {
    try {
      const updatedRow = rows.find((row) => row.id === id);
      if (updatedRow) {
        await processRowUpdate(updatedRow);
        setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.View } });
      }
    } catch (error) {
      console.error('Failed to save training centre:', error);
    }
  };
  

  const handleDeleteClick = (id: GridRowId) => async () => {    
    try {
      await deleteTrainingCentre(Number(id));
      setRows(rows.filter((row) => row.id !== id));
    }
    catch (error){
      console.error('Failed to delete course:', error);
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

  const mapWithAddressToTrainingCentre = (row: GridRowModel): TrainingCentreWithAddress => {
    return {
      id: row.id as number,
      name: row.name as string,
      addressId: row.addressId as number,
      street: row.street as string,
      number: row.number as string,
      postcode: row.postcode as string,
      city: row.city as string
    };
  };
  
  const processRowUpdate = async (newRow: GridRowModel) => {
    const updatedRow = { ...newRow, isNew: false };
    try {
      if (newRow.isNew) {
        const trainingCentre = mapWithAddressToTrainingCentre(newRow as TrainingCentreWithAddress);
        trainingCentre.id = 0;
        const createdCentre = await addTrainingCentre(trainingCentre);
        setRows(rows.map((row) => (row.id === newRow.id ? createdCentre : row)));
      } else {
        setRows(rows.map((row) => (row.id === newRow.id ? updatedRow : row)));
      }
    } catch (error) {
      console.error('Failed to add training centre:', error);
    }
    return updatedRow;
  };
  
  
  const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
    setRowModesModel(newRowModesModel);
  };

  const columns: GridColDef[] = [
    {
      field: "id",
      headerName: "#",
      editable: true,
      type: "number",
      headerAlign: 'center', // Aligns the header text to the center
      align: 'center', // Aligns the cell content to the center
      flex: 0.5,

    },
    {
      field: 'name',
      headerName: 'NAME',
      flex: 1,
      editable: true,
    },
    {
      field: 'addressId',
      headerName: 'ADDRESS ID',
      flex: 0.5,
      editable: false,
      headerAlign: 'center',
      align: 'center',
    },
    {
      field: 'street',
      headerName: 'STREET',
      flex: 1,
      editable: true,
    },
    {
      field: 'number',
      headerName: 'NUMBER',
      flex: 0.5,
      editable: true,
    },
    {
      field: 'postcode',
      headerName: 'POSTCODE',
      flex: 0.5,
      editable: true,
    },
    {
      field: 'city',
      headerName: 'CITY',
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
        </MainCard>
      );
    }      