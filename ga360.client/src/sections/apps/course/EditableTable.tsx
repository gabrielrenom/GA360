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

const initialRows: GridRowsProp = [
  {
    id: 1,
    name: "Course 1",
    description: "Description 1",
    duration: 2,
    registration: new Date(2023, 7, 1),
    expected: new Date(2024, 7, 1),
    certification: new Date(2024, 7, 1),
  },
  {
    id: 2,
    name: "Course 2",
    description: "Description 2",
    duration: 3,
    registration: new Date(2023, 8, 1),
    expected: new Date(2024, 8, 1),
    certification: new Date(2024, 8, 1),
  },
  // Add more rows as needed
];

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
        Add Course
      </Button>
    </GridToolbarContainer>
  );
}

export default function EditableTable() {
  const theme = useTheme();
  const downSM = useMediaQuery(theme.breakpoints.down('sm'));

  const [rows, setRows] = React.useState(initialRows);
  const [rowModesModel, setRowModesModel] = React.useState<GridRowModesModel>({});

  const handleRowEditStop: GridEventListener<"rowEditStop"> = (params, event) => {
    if (params.reason === GridRowEditStopReasons.rowFocusOut) {
      event.defaultMuiPrevented = true;
    }
  };

  const handleEditClick = (id: GridRowId) => () => {
    setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.Edit } });
  };

  const handleSaveClick = (id: GridRowId) => () => {
    setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.View } });
  };

  const handleDeleteClick = (id: GridRowId) => () => {
    setRows(rows.filter((row) => row.id !== id));
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

  const processRowUpdate = (newRow: GridRowModel) => {
    const updatedRow = { ...newRow, isNew: false };
    setRows(rows.map((row) => (row.id === newRow.id ? updatedRow : row)));
    return updatedRow;
  };

  const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
    setRowModesModel(newRowModesModel);
  };

  const columns: GridColDef[] = [
    {   
        field: "name",
        headerName: "NAME",
        flex: 1,
        editable: true,
    },
    {
      field: "description",
      headerName: "DESCRIPTION",
      flex: 1,
      editable: true,
    },
    {
      field: "duration",
      headerName: "HOURS",
      type: "number",
      flex: 1,
      editable: true,
    },
    {
      field: "registration",
      headerName: "REGISTRATION",
      type: "date",
      flex: 1,
      editable: true,
    },
    {
      field: "expected",
      headerName: "EXPECTED",
      type: "date",
      flex: 1,
      editable: true,
    },
    {
      field: "certification",
      headerName: "CERTIFICATION DATE",
      type: "date",
      flex: 2,
      editable: true,
    },
    {
      field: "certificationNumber",
      headerName: "CERT NUMBER",
      flex: 1,
      editable: true,
    },
    {
      field: "status",
      headerName: "STATUS",
      flex: 1,
      editable: true,
      type: "singleSelect",
      valueOptions: [
        { value: 1, label: "Active" },
        { value: 2, label: "Not Active" },
      ],
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
                    paddingLeft: 1, // Set the color of the first column header to red
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
