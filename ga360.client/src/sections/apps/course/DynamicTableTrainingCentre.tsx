import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import { useTheme } from '@mui/material/styles';
import System360LogoPNG from 'components/logo/SYSTEM360_Logo_Dark.png'
import {
  PlusOutlined,
  EditOutlined,
  DeleteOutlined,
  SaveOutlined,
  CloseOutlined,
  CameraOutlined,
  QuestionCircleOutlined,
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
  GridRowParams,
} from "@mui/x-data-grid";
import MainCard from "components/MainCard";
import { FormLabel, Stack, Tooltip, useMediaQuery } from "@mui/material";
import IconButton from "components/@extended/IconButton";
import { addTrainingCentre, deleteTrainingCentre, getTrainingCentres, TrainingCentre, TrainingCentreWithAddress, updateTrainingCentre } from "api/trainingcentreService";
import LogoUpload from "./components/LogoUpload";
import { useEffect, useState } from "react";


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
  const [imageURL, setImageURL] = React.useState<string | undefined>(undefined);

  React.useEffect(() => {
    const fetchTrainingCentres = async () => {
      try {
        const trainingCentres: TrainingCentre[] = await getTrainingCentres();
  
        const trainingCentresWithAddress: TrainingCentreWithAddress[] = trainingCentres.map(mapTrainingCentreToWithAddress);

        setRows(trainingCentresWithAddress);
      } catch (error) {
        console.error('Failed to fetch training centres:', error);
      }
    };
  
    fetchTrainingCentres();
  }, []);
  

  const mapTrainingCentreToWithAddress = (centre: TrainingCentre): TrainingCentreWithAddress => {
    const { id, name, addressId, address, logo } = centre;
    const { street, number, postcode, city } = address;
    return {
      id,
      name,
      addressId,
      street,
      number,
      postcode,
      city,
      logo
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

  
  const handleSaveClick = (id: GridRowId) => () => {
    setRowModesModel((prevRowModesModel) => ({
      ...prevRowModesModel,
      [id]: { mode: GridRowModes.View },
    }));
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
      city: row.city as string,
      logo: row.logo as string
    };
  };
  
  const processRowUpdate = async (newRow: GridRowModel) => {
    const updatedRow = { ...newRow, isNew: false };
    console.log("processRowUpdate");
  
    try {
      if (newRow.isNew) {
        const trainingCentre = mapWithAddressToTrainingCentre(newRow as TrainingCentreWithAddress);
        trainingCentre.id = 0;
        if (!trainingCentre.logo) { // Check if logo is undefined or empty
          trainingCentre.logo = System360LogoPNG; // Assign the default PNG image
      }

        const createdCentre = await addTrainingCentre(trainingCentre);
        setRows((prevRows) => prevRows.map((row) => (row.id === newRow.id ? createdCentre : row)));
      } else {
        const trainingCentre = mapWithAddressToTrainingCentre(newRow as TrainingCentreWithAddress);
        const updatedCentre = await updateTrainingCentre(trainingCentre.id, trainingCentre);
        setRows((prevRows) => prevRows.map((row) => (row.id === newRow.id ? updatedCentre : row)));
      }
    } catch (error) {
      console.error('Failed to update training centre:', error);
    }
    return updatedRow;
  };
  
  const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
    setRowModesModel(newRowModesModel);
  };

  const handleFileUpload = (file: File, setImageUrl: React.Dispatch<React.SetStateAction<string | undefined>>, id: GridRowParams['id']) => {
    console.log('File uploaded:', file);
    const reader = new FileReader();
  
    reader.onload = (e: ProgressEvent<FileReader>) => {
      const newImageUrl = e.target?.result as string;
      setImageUrl(newImageUrl);
  
      const copyRows = [...rows]; // Create a copy of the rows
      const rowIndex = copyRows.findIndex(row => row.id === id);
      console.log("MY ID", id);
      console.log(rowIndex);
      if (rowIndex !== -1) {
        copyRows[rowIndex].logo = newImageUrl;
        setRows(copyRows);
      }
      
      console.log('Updated rows:', rows);
    };
  
    reader.readAsDataURL(file);
  };
  
  const columns = [
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
      field: 'logo',
      //headerName: 'Logo',
      flex: 0.5,
      
      headerName: (
        
        <div style={{ display: 'flex', alignItems: 'center' }}>
          Logo
            <IconButton size="small" sx={{ ml: 0.5 }}>
            <QuestionCircleOutlined />
            </IconButton>
            We recomend PNG with 2084 × 903 pixels 
        </div>
      ),
      renderCell: (params) => {
        const [imageUrl, setImageUrl] = useState<string | undefined>(params.value);
        const rowId = params.id; // Explicitly get the row ID from params
  
        useEffect(() => {
          setImageUrl(params.value);
        }, [params.value]);
  
        const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
          const file = e.target.files?.[0];
          if (file) {
            handleFileUpload(file, setImageUrl, rowId); // Pass id to handleFileUpload
          }
        };
  
        console.log("MY ROW FROM INSIDE", rowId);
        return (
          <div onClick={handleEditClick(params.id)}>
          <Stack direction="row" justifyContent="center" sx={{ mt: 0.5, mr: 5 }}>
            <FormLabel
              htmlFor={`change-logo-${rowId}`} // Unique ID for each row
              sx={{
                position: 'relative',
                borderRadius: '50%',
                overflow: 'hidden',
                '&:hover .MuiBox-root': { opacity: 1 },
                cursor: 'pointer',
              }}
            >
              <Box
                sx={{
                  width: '100%',
                  height: '100%',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  position: 'relative',
                  borderRadius: '50%',
                }}
              >
                {imageUrl ? <CameraOutlined style={{ fontSize: '2rem', opacity: 0}} />:<CameraOutlined style={{ fontSize: '2rem'}} />}

                {imageUrl && (
                  <img
                    src={imageUrl}
                    alt="Uploaded logo"
                    style={{
                      position: 'absolute',
                      top: 0,
                      left: 0,
                      width: '100%',
                      height: '100%',
                      borderRadius: '50%',
                      objectFit: 'cover',
                    }}
                  />
                )}
              </Box>
            </FormLabel>
            <input
              type="file"
              id={`change-logo-${rowId}`} // Unique ID for each row
              style={{ display: 'none' }}
              accept="image/*"
              onChange={handleFileChange}
            />
          </Stack>
          </div>
        );
      },
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
      hideable:true
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
                //@ts-ignore
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
                      addressId: false,
                    },
                  },
                }}
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